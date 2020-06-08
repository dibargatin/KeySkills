using System;
using System.Collections.Generic;
using System.Linq;
using KeySkills.Core.Models;

namespace KeySkills.Core.Data.SeedData
{
    public class KeywordSeedData
    {
        public class Item
        {
            public Keyword Keyword { get; set; }
            public IEnumerable<string> PositiveTests { get; set; }
            public IEnumerable<string> NegativeTests { get; set; }
        }

        private static string WholeWord(string pattern) =>
            String.Concat(
                @"(^|[\p{C}\p{P}\p{S}\p{Z}]+)(",
                pattern, 
                @")([\p{C}\p{P}\p{S}\p{Z}]+|$)"
            );

        private static string[] Test(string text) => new[] {
            text, text.ToUpper(), text.ToLower(), $" {text}", $"{text} ", $" {text} ", $@"
            {text}", $",{text}", $"{text},", $",{text},", $"\t{text}", $"{text}\t", $"\t{text}\t", 
            $"<{text}", $"{text}>", $"<{text}>"
        };

        private static IEnumerable<string> NegativeTest(string text) => new[] {
            $"z{text}",$"{text}z",$"z{text}z"
        };

        private static IEnumerable<T> Collection<T>(params T[][] items) =>
            items.SelectMany(i => i);

        private static Item Keyword(
            int id, 
            string name, 
            string pattern, 
            IEnumerable<string> positiveTests, 
            IEnumerable<string> negativeTest
        ) =>
            new Item {
                Keyword = new Keyword {
                    KeywordId = id,
                    Name = name,
                    Pattern = WholeWord(pattern)
                },
                PositiveTests = positiveTests.SelectMany(text => Test(text)),
                NegativeTests = negativeTest.SelectMany(text => Test(text))
            };

        private static Item Keyword(int id, string name, string pattern, string[] tests) =>
            Keyword(id, name, pattern, 
                positiveTests: tests.SelectMany(text => Test(text)),
                negativeTest: tests.SelectMany(text => NegativeTest(text)).SelectMany(text => Test(text))
            );
        
        private static Item Keyword(int id, string name) =>
            Keyword(id, name,
                pattern: name.ToLower(),
                tests: new[] { name }
            );

        public IEnumerable<Item> Items => new[] {
            Keyword(1, ".NET", 
                pattern: @"\.net|dot\p{Z}*net", 
                tests: new[] {
                    ".Net","dotNet","Dot Net"
                }),
            Keyword(2, "ASP.NET",
                pattern: @"asp\p{Z}*\.\p{Z}*net",
                tests: new[] {
                    "ASP.Net","asp .NET","Asp. net"
                }),
            Keyword(3, "C#",
                pattern: @"c#|c\p{Z}*sharp",
                tests: new[] {
                    "c#","CSharp","c sharp","C  sharp"
                }),
            Keyword(4, "F#",
                pattern: @"f#|f\p{Z}*sharp",
                tests: new[] {
                    "f#","FSharp","f sharp","F  sharp"
                }),
            Keyword(5, "VB.NET",
                pattern: @"vb\p{Z}*\.\p{Z}*net",
                tests: new[] {
                    "Vb.Net","VB .NET","vb. net"
                }),
            Keyword(6, "WPF",
                pattern: @"wpf|windows\p{Z}*presentation\p{Z}*foundation",
                tests: new[] {
                    "Wpf",
                    "Windows Presentation Foundation",
                    "WindowsPresentationFoundation"
                }),
            Keyword(7, "WinForms",
                pattern: @"winforms|windows\p{Z}*forms",
                tests: new[] {
                    "WinForms","Windows Forms","Windows  Forms","WindowsForms"
                }),
            Keyword(8, "WCF",
                pattern: @"wcf|windows\p{Z}*communication\p{Z}*foundation",
                tests: new[] {
                    "Wcf",
                    "Windows Communication Foundation",
                    "WindowsCommunicationFoundation"
                }),
            Keyword(9, "ADO.NET",
                pattern: @"ado\p{Z}*\.\p{Z}*net",
                tests: new[] {
                    "ADO.Net","ado .NET","Ado. net"
                }),
            Keyword(10, "Entity Framework",
                pattern: @"ef|entity\p{Z}*framework",
                tests: new[] {
                    "Ef","Entity Framework","EntityFramework"
                }),
            Keyword(11, "Dapper"),
            Keyword(12, "NHibernate"),
            Keyword(13, "Xamarin"),
            Keyword(14, "JavaScript",
                pattern: @"js|javascript",
                tests: new[] {
                    "JavaScript","Js"
                }),
            Keyword(15, "TypeScript"),
            Keyword(16, "ReactJS",
                pattern: @"react|react\p{P}*\p{Z}*js",
                positiveTests: new[] {
                    "ReactJS","React js","React.JS","React"
                },
                negativeTest: new[] {
                    "xReactJS","xReact js","xReact.JS","xReact",
                    "xReactJSx","xReact jsx","xReact.JSx","xReactx",
                    "ReactJSx","Reactx"
                }),
            Keyword(17, "Angular"),
            Keyword(18, "AngularJS",
                pattern: @"angular\p{P}*\p{Z}*js",
                tests: new[] {
                    "AngularJS","Angular JS","Angular.JS"
                }),
            Keyword(19, "Vue.js",
                pattern: @"vue|vue\p{P}*\p{Z}*js",
                positiveTests: new[] {
                    "Vue","VueJS","Vue JS","Vue.JS"
                },
                negativeTest: new[] {
                    "xVue","xVueJS","xVue JS","xVue.JS",
                    "xVuex","xVueJSx","xVue JSx","xVue.JSx",
                    "Vuex","VueJSx"
                }),
            Keyword(20, "Node.js",
                pattern: @"node\p{P}*\p{Z}*js",
                tests: new[] {
                    "NodeJS","Node JS","Node.JS"
                }),
            Keyword(21, "JQuery"),
            Keyword(22, "Redux"),
            Keyword(23, "Svelte"),
            Keyword(24, "Express"),
            Keyword(25, "Nuxt"),
            Keyword(26, "Gulp"),
            Keyword(27, "Grunt"),
            Keyword(28, "Webpack"),
            Keyword(29, "Apollo"),
            Keyword(30, "Ionic"),
            Keyword(31, "Electron"),
            Keyword(32, "Java"),
            Keyword(33, "Spring"),
            Keyword(34, "Maven"),
            Keyword(35, "Gradle"),
            Keyword(36, "Python"),
            Keyword(37, "Django"),
            Keyword(38, "Flask"),
            Keyword(39, "SQL",
                pattern: @"sql|pgsql",
                tests: new[] {
                    "SQL","pgSQL"
                }),
            Keyword(40, "PostgreSQL",
                pattern: @"postgresql|postgres",
                tests: new[] { 
                    "PostgreSQL","Postgres"
                }),
            Keyword(41, "MySQL"),
            Keyword(42, "Oracle"),
            Keyword(43, "SQL Server",
                pattern: @"sql\p{P}*\p{Z}*server",
                tests: new[] {
                    "SQL Server","SqlServer","SQL-Server"
                }),
            Keyword(44, "MariaDB"),
            Keyword(45, "SQLite"),
            Keyword(46, "Db2"),
            Keyword(47, "MongoDB"),
            Keyword(48, "Redis"),
            Keyword(49, "NoSQL"),
            Keyword(50, "Prometheus"),
            Keyword(51, "InfluxDB"),
            Keyword(52, "HTML",
                pattern: @"html\d?|xhtml",
                tests: new[] {
                    "Html","Html5","xHtml"
                }),
            Keyword(53, "JSON"),
            Keyword(54, "XML"),
            Keyword(55, "CSS"),
            Keyword(56, "Less"),
            Keyword(57, "Sass"),
            Keyword(58, "Bootstrap"),
            Keyword(59, "Ruby"),
            Keyword(60, "Ruby On Rails",
                pattern: @"rails|ror|ruby\p{P}*\p{Z}*on\p{P}*\p{Z}*rails",
                positiveTests: new[] {
                    "Rails","RoR","Ruby On Rails","Ruby-on-rails"
                },
                negativeTest: new[] {
                    "Railsx","RoRx","Ruby On Railsx","Ruby-on-railsx",
                    "xRailsx","xRoRx","xRuby On Railsx","xRuby-on-railsx",
                    "xRails","xRoR"
                }),
            Keyword(61, "Sinatra"),
            Keyword(62, "C++",
                pattern: @"c\+\+|cpp|c\p{P}*\p{Z}*plus\p{P}*\p{Z}*plus",
                tests: new[] {
                    "C++","Cpp","C-plus-plus", "c plus plus"
                }),
            Keyword(63, "PHP"),
            Keyword(64, "Laravel"),
            Keyword(65, "Symfony"),
            Keyword(66, "CakePHP"),
            Keyword(67, "Yii"),
            Keyword(68, "Zend"),
            Keyword(69, "Drupal"),
            Keyword(70, "Scala"),
            Keyword(71, "Sbt"),
            Keyword(72, "Akka"),
            Keyword(73, "Spark"),
            Keyword(74, "Kafka"),
            Keyword(75, "Flink"),
            Keyword(76, "Hadoop"),
            Keyword(77, "Kotlin"),
            Keyword(78, "Golang",
                pattern: "go|golang",
                tests: new[] {
                    "Go","Golang"
                }),
            Keyword(79, "ECMAScript",
                pattern: @"ecma\p{P}*\p{Z}*script|es\d?",
                tests: new[] {
                    "ECMAScript","ECMA-Script","ES6"
                }),
            Keyword(80, "Clojure"),
            Keyword(81, "ClojureScript"),
            Keyword(82, "Lisp"),
            Keyword(83, "Rust"),
            Keyword(84, "Elm"),
            Keyword(85, "Haskell"),
            Keyword(86, "RabbitMQ"),
            Keyword(87, "Elasticsearch"),
            Keyword(88, "Lucene"),
            Keyword(89, "Docker"),
            Keyword(90, "Kubernetes"),
            Keyword(91, "Mesos"),
            Keyword(92, "AWS",
                pattern: @"aws|amazon\p{P}*\p{Z}*web\p{P}*\p{Z}*services",
                tests: new[] {
                    "Aws","Amazon Web Services","Amazon-Web-Services"
                }),
            Keyword(93, "Azure"),
            Keyword(94, "GCP",
                pattern: @"gcp|google\p{P}*\p{Z}*cloud\p{P}*\p{Z}*platform",
                tests: new[] {
                    "Gcp","Google Cloud Platform","Google-Cloud-Platform"
                }),
            Keyword(95, "Heroku"),
            Keyword(96, "OpenShift"),
            Keyword(97, "Cloud Foundry",
                pattern: @"cloud\p{P}*\p{Z}*foundry",
                tests: new[] {
                    "Cloud Foundry","Cloud-Foundry","CloudFoundry"
                }),
            Keyword(98, "Jenkins"),
            Keyword(99, "JIRA"),
            Keyword(100, "iOS"),
            Keyword(101, "macOS"),
            Keyword(102, "Swift"),
            Keyword(103, "Objective-C",
                pattern: @"objective\p{P}*\p{Z}*c",
                tests: new[] {
                    "Objective-C","ObjectiveC","Objective C"
                }),
            Keyword(104, "Android"),
            Keyword(105, "Windows"),
            Keyword(106, "Linux"),
            Keyword(107, "Debian"),
            Keyword(108, "Ubuntu"),
            Keyword(109, "Fedora"),
            Keyword(110, "CentOS"),
            Keyword(111, "RedHat"),
            Keyword(112, "Unix"),
            Keyword(113, "SOLID"),
            Keyword(114, "Microservices"),
            Keyword(115, "REST"),
            Keyword(116, "GraphQL"),
            Keyword(117, "gRPC")
        };
    }
}