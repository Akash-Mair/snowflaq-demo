module Index

open Elmish
open RicknMorty

type AsyncOperationStatus<'t> =
    | Started
    | Finished of 't

type Deferred<'t> =
    | HasNotStartedYet
    | InProgress
    | Resolved of 't
type Status =
    | Alive
    | Dead
    | Unknown
type RickNMortyCharacter =
    { Name: string
      Image: string
      Status: Status
      Id: string
      Species: string }
type Model = { Characters: Deferred<GetCharacters.Character list>  }

type Msg =
    | GetRickAndMortyCharacters of AsyncOperationStatus<Result<GetCharacters.Query,ErrorType list>>

let init () : Model * Cmd<Msg> =
    let model = { Characters = InProgress }
    let client = RicknMortyGraphqlClient("https://rickandmortyapi.com/graphql")
    let cmd =
        Cmd.OfAsync.perform client.GetCharacters () (Finished >> GetRickAndMortyCharacters)
    model, cmd

let update (msg: Msg) (model: Model) : Model * Cmd<Msg> =
    match msg with
    | GetRickAndMortyCharacters (Finished (Ok queryResult)) ->
        match queryResult.characters with
        | Some characters ->
            match characters.results with
            | Some x ->
                x
                |> List.choose id
                |> fun y -> { model with Characters = Resolved y }, Cmd.none
        | None -> model, Cmd.none

    | GetRickAndMortyCharacters (Finished (Error e)) -> model, Cmd.none

open Feliz
open Feliz.Bulma

let navBar =
    Bulma.navbar [
        prop.style [
            style.backgroundColor "#FF3CAC"
            style.backgroundImage "linear-gradient(225deg, #FF3CAC 0%, #784BA0 50%, #2B86C5 100%)"
        ]
        prop.children [
            Bulma.navbarBrand.div [
                Bulma.navbarItem.a [
                    prop.style [ style.color "white"; style.fontSize 20 ]
                    prop.text "Client side GraphQL"
                ]
            ]
        ]
    ]

let characterView (character: GetCharacters.Character) =
    Bulma.box [
        prop.style [ style.height 300; style.width 200; style.boxShadow(1, 1, 6, -1, "black") ]
        prop.children [
            Html.img [ prop.src character.image.Value ]
            Html.div (character.name.Value)
            Html.div (character.species.Value)
            Html.div (character.status.Value)
        ]
    ]


let charactersView (characters: GetCharacters.Character list) (dispatch: Msg -> unit) =
    Bulma.columns [
        columns.isMultiline
        prop.children [
            for character in characters do
                Bulma.column [
                characterView character
            ]
        ]
    ]

let view (model: Model) (dispatch: Msg -> unit) =
    Html.div [
        prop.style [ style.backgroundColor "lightgray" ]
        prop.children [
            navBar
            Bulma.container [
                prop.style [ style.margin 50;  style.display.flex; style.justifyContent.center; style.alignContent.center ]
                prop.children [
                    match model.Characters with
                    | HasNotStartedYet -> Html.div "Has not started"
                    | InProgress -> Html.div "Loading"
                    | Resolved characters -> charactersView characters dispatch
                ]
            ]
        ]
    ]
