using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KeySkills.Core.Data.Sqlite.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Keywords",
                columns: table => new
                {
                    KeywordId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false),
                    Pattern = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keywords", x => x.KeywordId);
                });

            migrationBuilder.CreateTable(
                name: "Vacancies",
                columns: table => new
                {
                    VacancyId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Link = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    PublishedAt = table.Column<DateTime>(nullable: false),
                    CountryCode = table.Column<string>(maxLength: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vacancies", x => x.VacancyId);
                });

            migrationBuilder.CreateTable(
                name: "VacancyKeywords",
                columns: table => new
                {
                    VacancyId = table.Column<int>(nullable: false),
                    KeywordId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VacancyKeywords", x => new { x.VacancyId, x.KeywordId });
                    table.ForeignKey(
                        name: "FK_VacancyKeywords_Keywords_KeywordId",
                        column: x => x.KeywordId,
                        principalTable: "Keywords",
                        principalColumn: "KeywordId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VacancyKeywords_Vacancies_VacancyId",
                        column: x => x.VacancyId,
                        principalTable: "Vacancies",
                        principalColumn: "VacancyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 1, ".NET", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(\\.net|dot\\p{Z}*net)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 85, "Haskell", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(haskell)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 84, "Elm", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(elm)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 83, "Rust", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(rust)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 82, "Lisp", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(lisp)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 81, "ClojureScript", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(clojurescript)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 80, "Clojure", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(clojure)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 79, "ECMAScript", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(ecma\\p{P}*\\p{Z}*script|es\\d?)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 78, "Golang", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(go|golang)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 77, "Kotlin", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(kotlin)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 76, "Hadoop", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(hadoop)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 75, "Flink", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(flink)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 74, "Kafka", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(kafka)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 73, "Spark", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(spark)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 72, "Akka", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(akka)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 71, "Sbt", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(sbt)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 70, "Scala", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(scala)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 69, "Drupal", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(drupal)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 68, "Zend", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(zend)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 67, "Yii", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(yii)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 66, "CakePHP", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(cakephp)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 65, "Symfony", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(symfony)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 64, "Laravel", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(laravel)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 63, "PHP", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(php)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 62, "C++", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(c\\+\\+|cpp|c\\p{P}*\\p{Z}*plus\\p{P}*\\p{Z}*plus)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 61, "Sinatra", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(sinatra)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 86, "RabbitMQ", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(rabbitmq)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 87, "Elasticsearch", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(elasticsearch)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 88, "Lucene", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(lucene)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 89, "Docker", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(docker)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 115, "REST", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(rest)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 114, "Microservices", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(microservices)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 113, "SOLID", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(solid)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 112, "Unix", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(unix)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 111, "RedHat", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(redhat)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 110, "CentOS", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(centos)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 109, "Fedora", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(fedora)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 108, "Ubuntu", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(ubuntu)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 107, "Debian", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(debian)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 106, "Linux", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(linux)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 105, "Windows", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(windows)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 104, "Android", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(android)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 60, "Ruby On Rails", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(rails|ror|ruby\\p{P}*\\p{Z}*on\\p{P}*\\p{Z}*rails)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 103, "Objective-C", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(objective\\p{P}*\\p{Z}*c)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 101, "macOS", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(macos)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 100, "iOS", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(ios)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 99, "JIRA", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(jira)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 98, "Jenkins", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(jenkins)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 97, "Cloud Foundry", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(cloud\\p{P}*\\p{Z}*foundry)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 96, "OpenShift", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(openshift)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 95, "Heroku", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(heroku)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 94, "GCP", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(gcp|google\\p{P}*\\p{Z}*cloud\\p{P}*\\p{Z}*platform)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 93, "Azure", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(azure)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 92, "AWS", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(aws|amazon\\p{P}*\\p{Z}*web\\p{P}*\\p{Z}*services)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 91, "Mesos", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(mesos)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 90, "Kubernetes", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(kubernetes)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 102, "Swift", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(swift)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 116, "GraphQL", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(graphql)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 59, "Ruby", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(ruby)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 57, "Sass", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(sass)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 26, "Gulp", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(gulp)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 25, "Nuxt", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(nuxt)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 24, "Express", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(express)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 23, "Svelte", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(svelte)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 22, "Redux", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(redux)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 21, "JQuery", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(jquery)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 20, "Node.js", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(node\\p{P}*\\p{Z}*js)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 19, "Vue.js", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(vue|vue\\p{P}*\\p{Z}*js)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 18, "AngularJS", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(angular\\p{P}*\\p{Z}*js)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 17, "Angular", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(angular)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 16, "ReactJS", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(react|react\\p{P}*\\p{Z}*js)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 15, "TypeScript", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(typescript)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 14, "JavaScript", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(js|javascript)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 13, "Xamarin", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(xamarin)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 12, "NHibernate", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(nhibernate)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 11, "Dapper", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(dapper)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 10, "Entity Framework", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(ef|entity\\p{Z}*framework)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 9, "ADO.NET", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(ado\\p{Z}*\\.\\p{Z}*net)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 8, "WCF", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(wcf|windows\\p{Z}*communication\\p{Z}*foundation)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 7, "WinForms", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(winforms|windows\\p{Z}*forms)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 6, "WPF", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(wpf|windows\\p{Z}*presentation\\p{Z}*foundation)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 5, "VB.NET", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(vb\\p{Z}*\\.\\p{Z}*net)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 4, "F#", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(f#|f\\p{Z}*sharp)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 3, "C#", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(c#|c\\p{Z}*sharp)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 2, "ASP.NET", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(asp\\p{Z}*\\.\\p{Z}*net)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 27, "Grunt", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(grunt)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 28, "Webpack", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(webpack)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 29, "Apollo", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(apollo)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 30, "Ionic", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(ionic)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 56, "Less", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(less)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 55, "CSS", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(css)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 54, "XML", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(xml)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 53, "JSON", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(json)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 52, "HTML", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(html\\d?|xhtml)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 51, "InfluxDB", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(influxdb)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 50, "Prometheus", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(prometheus)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 49, "NoSQL", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(nosql)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 48, "Redis", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(redis)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 47, "MongoDB", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(mongodb)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 46, "Db2", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(db2)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 45, "SQLite", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(sqlite)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 58, "Bootstrap", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(bootstrap)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 44, "MariaDB", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(mariadb)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 42, "Oracle", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(oracle)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 41, "MySQL", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(mysql)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 40, "PostgreSQL", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(postgresql|postgres)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 39, "SQL", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(sql|pgsql)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 38, "Flask", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(flask)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 37, "Django", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(django)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 36, "Python", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(python)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 35, "Gradle", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(gradle)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 34, "Maven", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(maven)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 33, "Spring", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(spring)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 32, "Java", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(java)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 31, "Electron", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(electron)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 43, "SQL Server", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(sql\\p{P}*\\p{Z}*server)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.InsertData(
                table: "Keywords",
                columns: new[] { "KeywordId", "Name", "Pattern" },
                values: new object[] { 117, "gRPC", "(^|[\\p{C}\\p{P}\\p{S}\\p{Z}]+)(grpc)([\\p{C}\\p{P}\\p{S}\\p{Z}]+|$)" });

            migrationBuilder.CreateIndex(
                name: "IX_Keywords_Name",
                table: "Keywords",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vacancies_Link",
                table: "Vacancies",
                column: "Link",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VacancyKeywords_KeywordId",
                table: "VacancyKeywords",
                column: "KeywordId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VacancyKeywords");

            migrationBuilder.DropTable(
                name: "Keywords");

            migrationBuilder.DropTable(
                name: "Vacancies");
        }
    }
}
