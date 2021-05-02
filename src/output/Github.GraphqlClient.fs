namespace Github

open Fable.Remoting.Json
open Newtonsoft.Json
open Newtonsoft.Json.Linq
open System.Net.Http
open System.Text

type GraphqlInput<'T> = { query: string; variables: Option<'T> }
type GraphqlSuccessResponse<'T> = { data: 'T }
type GraphqlErrorResponse = { errors: ErrorType list }

type GithubGraphqlClient(url: string, httpClient: HttpClient) =
    let converter = FableJsonConverter() :> JsonConverter
    let settings = JsonSerializerSettings(DateParseHandling=DateParseHandling.None, Converters = [| converter |])

    new(url: string) = GithubGraphqlClient(url, new HttpClient())

    member _.GetPullRequestsAsync(input: GetPullRequests.InputVariables) =
        async {
            let query = """
                query GetPullRequests($org: String!) {
                  organization(login: $org) {
                    name
                    url
                    repositories(first: 1) {
                      nodes {
                        name
                        pullRequests(first: 1, states: MERGED) {
                          nodes {
                            number
                            title
                            url
                            body
                            author {
                              login
                              url
                            }
                            sameAuthor: author {
                              login
                              url
                            }
                            mergedBy {
                              login
                              resourcePath
                            }
                            reviews(last: 10, states: APPROVED) {
                              nodes {
                                author {
                                  __typename
                                  avatarUrl
                                  login
                                  ... on Bot {
                                    __typename
                                    id
                                    login
                                  }
                                  ... on User {
                                    __typename
                                    id
                                    bio
                                  }
                                }
                              }
                            }
                          }
                        }
                      }
                    }
                  }
                }
            """

            let inputJson = JsonConvert.SerializeObject({ query = query; variables = Some input }, [| converter |])

            let! response =
                httpClient.PostAsync(url, new StringContent(inputJson, Encoding.UTF8, "application/json"))
                |> Async.AwaitTask

            let! responseContent = Async.AwaitTask(response.Content.ReadAsStringAsync())
            let responseJson = JsonConvert.DeserializeObject<JObject>(responseContent, settings)

            match response.IsSuccessStatusCode with
            | true ->
                let errorsReturned =
                    responseJson.ContainsKey "errors"
                    && responseJson.["errors"].Type = JTokenType.Array
                    && responseJson.["errors"].HasValues

                if errorsReturned then
                    let response = responseJson.ToObject<GraphqlErrorResponse>(JsonSerializer.Create(settings))
                    return Error response.errors
                else
                    let response = responseJson.ToObject<GraphqlSuccessResponse<GetPullRequests.Query>>(JsonSerializer.Create(settings))
                    return Ok response.data

            | errorStatus ->
                let response = responseJson.ToObject<GraphqlErrorResponse>(JsonSerializer.Create(settings))
                return Error response.errors
        }

    member this.GetPullRequests(input: GetPullRequests.InputVariables) = Async.RunSynchronously(this.GetPullRequestsAsync input)

    member _.GithubSearchAsync() =
        async {
            let query = """
                query GithubSearch {
                  search(query:"Snowflaqe", type:REPOSITORY, first:10) {
                    nodes {
                      ... on Repository {
                        __typename
                        name
                        nameWithOwner
                      }
                      ... on App {
                        __typename
                        id
                        name
                      }
                
                      ... on Issue {
                        __typename
                        id
                        title
                      }
                    }
                  }
                }
            """

            let inputJson = JsonConvert.SerializeObject({ query = query; variables = None }, [| converter |])

            let! response =
                httpClient.PostAsync(url, new StringContent(inputJson, Encoding.UTF8, "application/json"))
                |> Async.AwaitTask

            let! responseContent = Async.AwaitTask(response.Content.ReadAsStringAsync())
            let responseJson = JsonConvert.DeserializeObject<JObject>(responseContent, settings)

            match response.IsSuccessStatusCode with
            | true ->
                let errorsReturned =
                    responseJson.ContainsKey "errors"
                    && responseJson.["errors"].Type = JTokenType.Array
                    && responseJson.["errors"].HasValues

                if errorsReturned then
                    let response = responseJson.ToObject<GraphqlErrorResponse>(JsonSerializer.Create(settings))
                    return Error response.errors
                else
                    let response = responseJson.ToObject<GraphqlSuccessResponse<GithubSearch.Query>>(JsonSerializer.Create(settings))
                    return Ok response.data

            | errorStatus ->
                let response = responseJson.ToObject<GraphqlErrorResponse>(JsonSerializer.Create(settings))
                return Error response.errors
        }

    member this.GithubSearch() = Async.RunSynchronously(this.GithubSearchAsync())

    member _.SimplePullRequestsAsync(input: SimplePullRequests.InputVariables) =
        async {
            let query = """
                query SimplePullRequests ($org: String!) {
                  organization(login: $org) {
                    name
                    url
                    repositories(first: 1) {
                      nodes {
                        name
                        pullRequests(first: 1, states: MERGED) {
                          nodes {
                            number
                            title
                            url
                            body
                            author {
                              login
                              url
                            }
                            mergedBy {
                              login
                              resourcePath
                            }
                            reviews(last: 10, states: APPROVED) {
                              nodes {
                                author {
                                  avatarUrl
                                  login
                                }
                              }
                            }
                          }
                        }
                      }
                    }
                  }
                }
            """

            let inputJson = JsonConvert.SerializeObject({ query = query; variables = Some input }, [| converter |])

            let! response =
                httpClient.PostAsync(url, new StringContent(inputJson, Encoding.UTF8, "application/json"))
                |> Async.AwaitTask

            let! responseContent = Async.AwaitTask(response.Content.ReadAsStringAsync())
            let responseJson = JsonConvert.DeserializeObject<JObject>(responseContent, settings)

            match response.IsSuccessStatusCode with
            | true ->
                let errorsReturned =
                    responseJson.ContainsKey "errors"
                    && responseJson.["errors"].Type = JTokenType.Array
                    && responseJson.["errors"].HasValues

                if errorsReturned then
                    let response = responseJson.ToObject<GraphqlErrorResponse>(JsonSerializer.Create(settings))
                    return Error response.errors
                else
                    let response = responseJson.ToObject<GraphqlSuccessResponse<SimplePullRequests.Query>>(JsonSerializer.Create(settings))
                    return Ok response.data

            | errorStatus ->
                let response = responseJson.ToObject<GraphqlErrorResponse>(JsonSerializer.Create(settings))
                return Error response.errors
        }

    member this.SimplePullRequests(input: SimplePullRequests.InputVariables) = Async.RunSynchronously(this.SimplePullRequestsAsync input)
