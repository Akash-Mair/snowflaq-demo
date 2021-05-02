[<RequireQualifiedAccess>]
module rec Github.SimplePullRequests

type InputVariables = { org: string }

/// The actor who authored the comment.
type Actor =
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

/// The actor who authored the comment.
type Author =
    { /// A URL pointing to the actor's public avatar.
      avatarUrl: string
      /// The username of the actor.
      login: string }

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
