﻿namespace TypeProviderUser.SQLite
open Rezoom.SQL
open Rezoom.SQL.Mapping
open Rezoom.SQL.Migrations
open Rezoom.SQL.Synchronous
open System.IO

type TestModel = SQLModel<".">

type TestData = SQL<"""
insert into Pictures(SHA256, PNGData)
values  ( x'ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff'
        , x''
        );
insert into Pictures(SHA256, PNGData)
values  ( x'0000000000000000000000000000000000000000000000000000000000000000'
        , x''
        );
insert into Users(Name, Email, ProfilePictureSHA256)
values  ( 'Homer'
        , 'homer.simpson@springfieldnuclear.com'
        , x'ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff'
        );
insert into Users(Name, Email, ProfilePictureSHA256)
values  ( 'Marge'
        , 'marge@globex.com'
        , x'0000000000000000000000000000000000000000000000000000000000000000'
        );
insert into Articles(AuthorId, ArticleTitle, ArticleText)
values  ( (select Id from Users where Name = 'Homer')
        , 'My first review as a food critic.'
        , 'Mmmmmmm... donuts'
        );
insert into Articles(AuthorId, ArticleTitle, ArticleText)
values  ( (select Id from Users where Name = 'Homer')
        , 'My second review as a food critic.'
        , 'Mmmmmmm... beer'
        );
insert into ArticleComments(AuthorId, ArticleId, CommentText)
values  ( (select Id from Users where Name = 'Marge')
        , (select Id from Articles where ArticleTitle = 'My first review as a food critic.')
        , 'Are you sure you should be eating so many donuts?'
        );
insert into ArticleComments(AuthorId, ArticleId, CommentText)
values  ( (select Id from Users where Name = 'Marge')
        , (select Id from Articles where ArticleTitle = 'My second review as a food critic.')
        , 'Are you sure you should be drinking so many beers?'
        );
""">

[<AutoOpen>]
module Helpers =
    let dbFileName = "rzsql.db"
    let runOnTestData (cmd : Command<'a>) =
        if File.Exists(dbFileName) then
            File.Delete(dbFileName)
        TestModel.Migrate(MigrationConfig.Default)
        use cxt = new ConnectionContext()
        TestData.Command().Execute(cxt)
        cmd.Execute(cxt)