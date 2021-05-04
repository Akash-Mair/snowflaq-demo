namespace RickAndMorty

open Fable.SimpleHttp
open Fable.SimpleJson

type GraphqlInput<'T> = { query: string; variables: Option<'T> }
type GraphqlSuccessResponse<'T> = { data: 'T }
type GraphqlErrorResponse = { errors: ErrorType list }

type RickAndMortyGraphqlClient(url: string, headers: Header list) =
    new(url: string) = RickAndMortyGraphqlClient(url, [ ])

    member _.GetCharacters() =
        async {
            let query = """
                query GetCharacters {
                  characters {
                    results {
                      name
                      id
                      image
                      status
                      species
                    }
                  }
                }
            """
            let! response =
                Http.request url
                |> Http.method POST
                |> Http.headers [ Headers.contentType "application/json"; yield! headers ]
                |> Http.content (BodyContent.Text (Json.serialize { query = query; variables = None }))
                |> Http.send

            match response.statusCode with
            | 200 ->
                let response = Json.parseNativeAs<GraphqlSuccessResponse<GetCharacters.Query>> response.responseText
                return Ok response.data

            | errorStatus ->
                let response = Json.parseNativeAs<GraphqlErrorResponse> response.responseText
                return Error response.errors
        }
