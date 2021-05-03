[<RequireQualifiedAccess>]
module rec RicknMorty.GetCharacters

type Character =
    { /// The name of the character.
      name: Option<string>
      /// The id of the character.
      id: Option<string>
      /// Link to the character's image.All images are 300x300px and most are medium shots or portraits since they are intended to be used as avatars.
      image: Option<string>
      /// The status of the character ('Alive', 'Dead' or 'unknown').
      status: Option<string>
      /// The species of the character.
      species: Option<string> }

/// Get the list of all characters
type Characters =
    { results: Option<list<Option<Character>>> }

type Query =
    { /// Get the list of all characters
      characters: Option<Characters> }
