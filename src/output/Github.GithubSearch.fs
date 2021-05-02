[<RequireQualifiedAccess>]
module rec Github.GithubSearch

/// A repository contains the content for a project.
type Repository =
    { ///The name of the type
      __typename: string
      /// The name of the repository.
      name: string
      /// The repository's name with owner.
      nameWithOwner: string }

/// A GitHub App.
type App =
    { ///The name of the type
      __typename: string
      id: string
      /// The name of the app.
      name: string }

/// An Issue is a place to discuss ideas, enhancements, tasks, and bugs for a project.
type Issue =
    { ///The name of the type
      __typename: string
      id: string
      /// Identifies the issue title.
      title: string }

/// Perform a search across resources.
[<RequireQualifiedAccess>]
type SearchResultItem =
    | Repository of repository: Repository
    | App of app: App
    | Issue of issue: Issue

/// Perform a search across resources.
type SearchResultItemConnection =
    { /// A list of nodes.
      nodes: Option<list<Option<SearchResultItem>>> }

/// The query root of GitHub's GraphQL interface.
type Query =
    { /// Perform a search across resources.
      search: SearchResultItemConnection }
