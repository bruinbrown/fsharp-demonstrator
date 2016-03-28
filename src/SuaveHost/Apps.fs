﻿module SuaveHost.Apps

open Suave
open Suave.Files
open Suave.Filters
open Suave.Operators
open Suave.Writers
open System


open FootballDemo
open FootballDemo.LeagueTable
open FootballDemo.TeamStats

/// Routes for the Football Library.
let footballApp =
    GET >=> choose [
        pathScan "/api/leaguetable/%d" (enum<FootballMonth> >> getLeague >> toJsonAsync)
        pathScan "/api/team/%s" (Uri.UnescapeDataString >> loadStatsForTeam >> toJsonAsync) ]

/// Routes for the Enigma app.
let enigmaApp =
    POST >=> choose [
        path "/api/enigma/translate" >=> fun ctx ->
            async { return Some ctx } ]

/// Routes for non-application specific features.
let basicApp staticFileRoot =
    GET >=> choose [
        path "/throwAnException" >=> (fun _ -> failwith "Oh no! You've done something STUPID!"; async.Return None)
        path "/" >=> browseFile staticFileRoot "index.html"
        browse staticFileRoot ]

/// Routes through to 404.html
let pageNotFound staticFileRoot = browseFile staticFileRoot "404.html" >=> setStatus HttpCode.HTTP_404