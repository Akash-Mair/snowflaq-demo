[<RequireQualifiedAccess>]
module rec Github.GetPullRequests

type InputVariables = { org: string }

/// The actor who authored the comment.
type Actor =
    { /// The username of the actor.
      login: string
      /// The HTTP URL for this actor.
      url: string }

/// The actor who authored the comment.
type SameAuthor =
    { /// The username of the actor.
      login: string
      /// The HTTP URL for this actor.
      url: string }

/// The actor who merged the pull request.
type MergedBy =
    { /// The username of the actor.
      login: string
      /// The HTTP path for this actor.
      resourcePath: string }

/// An account for a user who is an admin of an enterprise or a member of an enterprise through one or more organizations.
type EnterpriseUserAccount =
    { ///The name of the type
      __typename: string
      /// A URL pointing to the enterprise user account's public avatar.
      avatarUrl: string
      /// An identifier for the enterprise user account, a login or email address
      login: string }

/// A placeholder user for attribution of imported data on GitHub.
type Mannequin =
    { ///The name of the type
      __typename: string
      /// A URL pointing to the GitHub App's public avatar.
      avatarUrl: string
      /// The username of the actor.
      login: string }

/// An account on GitHub, with one or more owners, that has repositories, members and teams.
type TypenameAndAvatarUrlAndLogin =
    { ///The name of the type
      __typename: string
      /// A URL pointing to the organization's public avatar.
      avatarUrl: string
      /// The organization's login name.
      login: string }

/// A special type of user which takes actions on behalf of GitHub Apps.
type Bot =
    { ///The name of the type
      __typename: string
      /// A URL pointing to the GitHub App's public avatar.
      avatarUrl: string
      /// The username of the actor.
      login: string
      id: string }

/// A user is an individual's account on GitHub that owns repositories and can make new content.
type User =
    { ///The name of the type
      __typename: string
      /// A URL pointing to the user's public avatar.
      avatarUrl: string
      /// The username used to login.
      login: string
      id: string
      /// The user's public profile bio.
      bio: Option<string> }

/// A list of nodes.
[<RequireQualifiedAccess>]
type Author =
    | EnterpriseUserAccount of enterpriseuseraccount: EnterpriseUserAccount
    | Mannequin of mannequin: Mannequin
    | Organization of organization: TypenameAndAvatarUrlAndLogin
    | Bot of bot: Bot
    | User of user: User

/// A list of nodes.
type PullRequestReview =
    { /// The actor who authored the comment.
      author: Option<Author> }

/// A list of reviews associated with the pull request.
type PullRequestReviewConnection =
    { /// A list of nodes.
      nodes: Option<list<Option<PullRequestReview>>> }

/// A list of nodes.
type PullRequest =
    { /// Identifies the pull request number.
      number: int
      /// Identifies the pull request title.
      title: string
      /// The HTTP URL for this pull request.
      url: string
      /// The body as Markdown.
      body: string
      /// The actor who authored the comment.
      author: Option<Actor>
      /// The actor who authored the comment.
      sameAuthor: Option<SameAuthor>
      /// The actor who merged the pull request.
      mergedBy: Option<MergedBy>
      /// A list of reviews associated with the pull request.
      reviews: Option<PullRequestReviewConnection> }

/// A list of pull requests that have been opened in the repository.
type PullRequestConnection =
    { /// A list of nodes.
      nodes: Option<list<Option<PullRequest>>> }

/// A list of nodes.
type Repository =
    { /// The name of the repository.
      name: string
      /// A list of pull requests that have been opened in the repository.
      pullRequests: PullRequestConnection }

/// A list of repositories that the user owns.
type RepositoryConnection =
    { /// A list of nodes.
      nodes: Option<list<Option<Repository>>> }

/// Lookup a organization by login.
type Organization =
    { /// The organization's public profile name.
      name: Option<string>
      /// The HTTP URL for this organization.
      url: string
      /// A list of repositories that the user owns.
      repositories: RepositoryConnection }

/// The query root of GitHub's GraphQL interface.
type Query =
    { /// Lookup a organization by login.
      organization: Option<Organization> }
