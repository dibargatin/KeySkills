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

        private static string[] Test(string word) => new[] {
            word, word.ToUpper(), word.ToLower(), $" {word}", $"{word} ", $" {word} ", $@"
            {word}", $",{word}", $"{word},", $",{word},", $"\t{word}", $"{word}\t", $"\t{word}\t", 
            $"<{word}", $"{word}>", $"<{word}>"
        };

        private static IEnumerable<T> Collection<T>(params T[][] items) =>
            items.SelectMany(i => i);

        public IEnumerable<Item> Items => new[] {
            new Item {
                Keyword = new Keyword {
                    KeywordId = 1,
                    Name = ".NET",
                    Pattern = WholeWord(@"\.net|dot\p{Z}*net")
                },
                PositiveTests = Collection(
                    Test(".Net"), 
                    Test(".NET Framework"), 
                    Test(".Net Core"), 
                    Test(".NeT 4"),
                    Test("dotNet"),
                    Test("Dot Net")
                ),
                NegativeTests = Collection(
                    Test("xnet"),
                    Test(".network"),
                    Test("asp.net"),
                    Test(".netx"),
                    Test("xdotnet"),
                    Test("dotnetx"),
                    Test("xdot net"),
                    Test("dot netx")
                )
            },
            new Item {
                Keyword = new Keyword {
                    KeywordId = 2,
                    Name = "ASP.NET",
                    Pattern = WholeWord(@"asp\p{Z}*\.\p{Z}*net")
                },
                PositiveTests = Collection(
                    Test("ASP.Net"), 
                    Test("asp .NET"),
                    Test("Asp. net"),
                    Test("Asp.Net Core"),
                    Test("Asp.Net 5")
                ),
                NegativeTests = Collection(
                    Test("xasp.net"),
                    Test("asp.network"),
                    Test("asp. network"),
                    Test("xasp.network")
                )
            },
            new Item {
                Keyword = new Keyword {
                    KeywordId = 3,
                    Name = "C#",
                    Pattern = WholeWord(@"c#|c\p{Z}*sharp")
                },
                PositiveTests = Collection(
                    Test("c#"),
                    Test("CSharp"),
                    Test("c sharp"),
                    Test("C  sharp")
                ),
                NegativeTests = Collection(
                    Test("xc#"),
                    Test("xcsharp"),
                    Test("xc sharp"),
                    Test("csharpx"),
                    Test("c sharpx")
                )
            },
            new Item {
                Keyword = new Keyword {
                    KeywordId = 4,
                    Name = "F#",
                    Pattern = WholeWord(@"f#|f\p{Z}*sharp")
                },
                PositiveTests = Collection(
                    Test("f#"),
                    Test("FSharp"),
                    Test("f sharp"),
                    Test("F  sharp")
                ),
                NegativeTests = Collection(
                    Test("xf#"),
                    Test("xfsharp"),
                    Test("xf sharp"),
                    Test("fsharpx"),
                    Test("f sharpx")
                )
            },
            new Item {
                Keyword = new Keyword {
                    KeywordId = 5,
                    Name = "VB.NET",
                    Pattern = WholeWord(@"vb\p{Z}*\.\p{Z}*net")
                },
                PositiveTests = Collection(
                    Test("Vb.Net"),
                    Test("VB .NET"),
                    Test("vb. net")
                ),
                NegativeTests = Collection(
                    Test("xvb.net"),
                    Test("xvb. net"),
                    Test("xvb .net"),
                    Test("vb.netx"),
                    Test("vb .netx"),
                    Test("vb. netx")
                )
            },
            new Item {
                Keyword = new Keyword {
                    KeywordId = 6,
                    Name = "WPF",
                    Pattern = WholeWord(@"wpf|windows\p{Z}*presentation\p{Z}*foundation")
                },
                PositiveTests = Collection(
                    Test("Wpf"),
                    Test("Windows Presentation Foundation"),
                    Test("WindowsPresentationFoundation")
                ),
                NegativeTests = Collection(
                    Test("xwpf"),
                    Test("wpfx"),
                    Test("xWindows Presentation Foundation"),
                    Test("Windows Presentation Foundationx"),
                    Test("xWindows Presentation Foundationx")
                )
            },
            new Item {
                Keyword = new Keyword {
                    KeywordId = 7,
                    Name = "WinForms",
                    Pattern = WholeWord(@"winforms|windows\p{Z}*forms")
                },
                PositiveTests = Collection(
                    Test("WinForms"),
                    Test("Windows Forms"),
                    Test("Windows  Forms"),
                    Test("WindowsForms")
                ),
                NegativeTests = Collection(
                    Test("xWinForms"),
                    Test("WinFormsx"),
                    Test("xWindows Forms"),
                    Test("Windows Formsx"),
                    Test("xWindows Formsx")
                )
            },
            new Item {
                Keyword = new Keyword {
                    KeywordId = 8,
                    Name = "WCF",
                    Pattern = WholeWord(@"wcf|windows\p{Z}*communication\p{Z}*foundation")
                },
                PositiveTests = Collection(
                    Test("Wcf"),
                    Test("Windows Communication Foundation"),
                    Test("WindowsCommunicationFoundation")
                ),
                NegativeTests = Collection(
                    Test("xwcf"),
                    Test("wcfx"),
                    Test("xWindows Communication Foundation"),
                    Test("Windows Communication Foundationx"),
                    Test("xWindows Communication Foundationx")
                )
            },
            new Item {
                Keyword = new Keyword {
                    KeywordId = 9,
                    Name = "ADO.NET",
                    Pattern = WholeWord(@"ado\p{Z}*\.\p{Z}*net")
                },
                PositiveTests = Collection(
                    Test("ADO.Net"), 
                    Test("ado .NET"),
                    Test("Ado. net")
                ),
                NegativeTests = Collection(
                    Test("xado.net"),
                    Test("ado.network"),
                    Test("ado. network"),
                    Test("xado.network")
                )
            },
            new Item {
                Keyword = new Keyword {
                    KeywordId = 10,
                    Name = "Entity Framework",
                    Pattern = WholeWord(@"ef|entity\p{Z}*framework")
                },
                PositiveTests = Collection(
                    Test("Ef"),
                    Test("Ef Core"),
                    Test("Entity Framework"),
                    Test("EntityFramework")
                ),
                NegativeTests = Collection(
                    Test("xef"),
                    Test("eFx"),
                    Test("xEntity Framework"),
                    Test("EntityFrameworkx")
                )
            },
            new Item {
                Keyword = new Keyword {
                    KeywordId = 11,
                    Name = "Dapper",
                    Pattern = WholeWord(@"dapper")
                },
                PositiveTests = Collection(
                    Test("Dapper")
                ),
                NegativeTests = Collection(
                    Test("xDapper"),
                    Test("dapperx"),
                    Test("daper")
                )
            },
            new Item {
                Keyword = new Keyword {
                    KeywordId = 12,
                    Name = "NHibernate",
                    Pattern = WholeWord(@"nhibernate")
                },
                PositiveTests = Collection(
                    Test("NHibernate")
                ),
                NegativeTests = Collection(
                    Test("xnhibernate"),
                    Test("nhibernatex"),
                    Test("hibernate")
                )
            },
            new Item {
                Keyword = new Keyword {
                    KeywordId = 13,
                    Name = "Xamarin",
                    Pattern = WholeWord(@"xamarin")
                },
                PositiveTests = Collection(
                    Test("Xamarin")
                ),
                NegativeTests = Collection(
                    Test("xxamarin"),
                    Test("xamarinx")
                )
            },
            new Item {
                Keyword = new Keyword {
                    KeywordId = 14,
                    Name = "JavaScript",
                    Pattern = WholeWord(@"js|javascript")
                },
                PositiveTests = Collection(
                    Test("JavaScript"),
                    Test("Js")
                ),
                NegativeTests = Collection(
                    Test("xJavaScript"),
                    Test("JavaScriptx"),
                    Test("jsx")
                )
            }, 
            new Item {
                Keyword = new Keyword {
                    KeywordId = 15,
                    Name = "TypeScript",
                    Pattern = WholeWord(@"typescript")
                },
                PositiveTests = Collection(
                    Test("TypeScript")
                ),
                NegativeTests = Collection(
                    Test("xTypeScript"),
                    Test("TypeScriptx")
                )
            },
            new Item {
                Keyword = new Keyword {
                    KeywordId = 16,
                    Name = "ReactJS",
                    Pattern = WholeWord(@"react|react\p{P}*\p{Z}*js")
                },
                PositiveTests = Collection(
                    Test("ReactJS"),
                    Test("React js"),
                    Test("React.JS"),
                    Test("React")
                ),
                NegativeTests = Collection(
                    Test("reactjsx"),
                    Test("xreactjs"),
                    Test("reactx")
                )
            },  
            new Item {
                Keyword = new Keyword {
                    KeywordId = 17,
                    Name = "Angular",
                    Pattern = WholeWord(@"angular")
                },
                PositiveTests = Collection(
                    Test("Angular")
                ),
                NegativeTests = Collection(
                    Test("AngularJS"),
                    Test("xAngular"),
                    Test("Angularx")
                )
            }, 
            new Item {
                Keyword = new Keyword {
                    KeywordId = 18,
                    Name = "AngularJS",
                    Pattern = WholeWord(@"angular\p{P}*\p{Z}*js")
                },
                PositiveTests = Collection(
                    Test("AngularJS"),
                    Test("Angular JS"),
                    Test("Angular.JS")
                ),
                NegativeTests = Collection(
                    Test("Angular"),
                    Test("xAngularJs"),
                    Test("AngularJSx")
                )
            },
            new Item {
                Keyword = new Keyword {
                    KeywordId = 19,
                    Name = "Vue.js",
                    Pattern = WholeWord(@"vue|vue\p{P}*\p{Z}*js")
                },
                PositiveTests = Collection(
                    Test("Vue"),
                    Test("VueJS"),
                    Test("Vue JS"),
                    Test("Vue.JS")
                ),
                NegativeTests = Collection(
                    Test("Vuex"),
                    Test("xVueJs"),
                    Test("VueJSx")
                )
            },
            new Item {
                Keyword = new Keyword {
                    KeywordId = 20,
                    Name = "Node.js",
                    Pattern = WholeWord(@"node\p{P}*\p{Z}*js")
                },
                PositiveTests = Collection(
                    Test("NodeJS"),
                    Test("Node JS"),
                    Test("Node.JS")
                ),
                NegativeTests = Collection(
                    Test("node"),
                    Test("xnodeJs"),
                    Test("node JSx")
                )
            },
            new Item {
                Keyword = new Keyword {
                    KeywordId = 21,
                    Name = "JQuery",
                    Pattern = WholeWord(@"jquery")
                },
                PositiveTests = Collection(
                    Test("JQuery")
                ),
                NegativeTests = Collection(
                    Test("xJQuery"),
                    Test("JQueryx")
                )
            },
            new Item {
                Keyword = new Keyword {
                    KeywordId = 22,
                    Name = "Redux",
                    Pattern = WholeWord(@"redux")
                },
                PositiveTests = Collection(
                    Test("Redux")
                ),
                NegativeTests = Collection(
                    Test("xRedux"),
                    Test("Reduxx")
                )
            },
            new Item {
                Keyword = new Keyword {
                    KeywordId = 23,
                    Name = "Svelte",
                    Pattern = WholeWord(@"svelte")
                },
                PositiveTests = Collection(
                    Test("Svelte")
                ),
                NegativeTests = Collection(
                    Test("xSvelte"),
                    Test("Sveltex")
                )
            },
            new Item {
                Keyword = new Keyword {
                    KeywordId = 24,
                    Name = "Express",
                    Pattern = WholeWord(@"express")
                },
                PositiveTests = Collection(
                    Test("Express")
                ),
                NegativeTests = Collection(
                    Test("xExpress"),
                    Test("expressx")
                )
            }, 
            new Item {
                Keyword = new Keyword {
                    KeywordId = 25,
                    Name = "Nuxt",
                    Pattern = WholeWord(@"nuxt")
                },
                PositiveTests = Collection(
                    Test("Nuxt")
                ),
                NegativeTests = Collection(
                    Test("xnuxt"),
                    Test("nuxtx")
                )
            },
            new Item {
                Keyword = new Keyword {
                    KeywordId = 26,
                    Name = "Gulp",
                    Pattern = WholeWord(@"gulp")
                },
                PositiveTests = Collection(
                    Test("Gulp")
                ),
                NegativeTests = Collection(
                    Test("xgulp"),
                    Test("gulpx")
                )
            }, 
            new Item {
                Keyword = new Keyword {
                    KeywordId = 27,
                    Name = "Grunt",
                    Pattern = WholeWord(@"grunt")
                },
                PositiveTests = Collection(
                    Test("Grunt")
                ),
                NegativeTests = Collection(
                    Test("xGrunt"),
                    Test("Gruntx")
                )
            },
            new Item {
                Keyword = new Keyword {
                    KeywordId = 28,
                    Name = "Webpack",
                    Pattern = WholeWord(@"webpack")
                },
                PositiveTests = Collection(
                    Test("Webpack")
                ),
                NegativeTests = Collection(
                    Test("xWebpack"),
                    Test("Webpackx")
                )
            },
            new Item {
                Keyword = new Keyword {
                    KeywordId = 29,
                    Name = "Apollo",
                    Pattern = WholeWord(@"apollo")
                },
                PositiveTests = Collection(
                    Test("Apollo")
                ),
                NegativeTests = Collection(
                    Test("xApollo"),
                    Test("Apollox")
                )
            },
            new Item {
                Keyword = new Keyword {
                    KeywordId = 30,
                    Name = "Ionic",
                    Pattern = WholeWord(@"ionic")
                },
                PositiveTests = Collection(
                    Test("Ionic")
                ),
                NegativeTests = Collection(
                    Test("xIonic"),
                    Test("Ionicx")
                )
            },
            new Item {
                Keyword = new Keyword {
                    KeywordId = 31,
                    Name = "Electron",
                    Pattern = WholeWord(@"electron")
                },
                PositiveTests = Collection(
                    Test("Electron")
                ),
                NegativeTests = Collection(
                    Test("xelectron"),
                    Test("electronx")
                )
            },
            new Item {
                Keyword = new Keyword {
                    KeywordId = 32,
                    Name = "Java",
                    Pattern = WholeWord(@"java")
                },
                PositiveTests = Collection(
                    Test("Java")
                ),
                NegativeTests = Collection(
                    Test("xJava"),
                    Test("Javax")
                )
            },
            new Item {
                Keyword = new Keyword {
                    KeywordId = 33,
                    Name = "Spring",
                    Pattern = WholeWord(@"spring")
                },
                PositiveTests = Collection(
                    Test("Spring")
                ),
                NegativeTests = Collection(
                    Test("xSpring"),
                    Test("Springx")
                )
            }, 
            new Item {
                Keyword = new Keyword {
                    KeywordId = 34,
                    Name = "Maven",
                    Pattern = WholeWord(@"maven")
                },
                PositiveTests = Collection(
                    Test("Maven")
                ),
                NegativeTests = Collection(
                    Test("xMaven"),
                    Test("Mavenx")  
                )
            },
            new Item {
                Keyword = new Keyword {
                    KeywordId = 35,
                    Name = "Gradle",
                    Pattern = WholeWord(@"gradle")
                },
                PositiveTests = Collection(
                    Test("Gradle")
                ),
                NegativeTests = Collection(
                    Test("xGradle"),
                    Test("Gradlex")
                )
            },
            new Item {
                Keyword = new Keyword {
                    KeywordId = 36,
                    Name = "Python",
                    Pattern = WholeWord(@"python")
                },
                PositiveTests = Collection(
                    Test("Python")
                ),
                NegativeTests = Collection(
                    Test("xPython"),
                    Test("Pythonx")
                )
            },
            new Item {
                Keyword = new Keyword {
                    KeywordId = 37,
                    Name = "django",
                    Pattern = WholeWord(@"django")
                },
                PositiveTests = Collection(
                    Test("django")
                ),
                NegativeTests = Collection(
                    Test("xdjango"),
                    Test("djangox")
                )
            },
            new Item {
                Keyword = new Keyword {
                    KeywordId = 38,
                    Name = "Flask",
                    Pattern = WholeWord(@"flask")
                },
                PositiveTests = Collection(
                    Test("Flask")
                ),
                NegativeTests = Collection(
                    Test("xFlask"),
                    Test("Flaskx")
                )
            },
            new Item {
                Keyword = new Keyword {
                    KeywordId = 39,
                    Name = "SQL",
                    Pattern = WholeWord(@"sql")
                },
                PositiveTests = Collection(
                    Test("SQL"),
                    Test("T-SQL"),
                    Test("PL/SQL")
                ),
                NegativeTests = Collection(
                    Test("noSQL"),
                    Test("SQLx")
                )
            },
            new Item {
                Keyword = new Keyword {
                    KeywordId = 40,
                    Name = "PostgreSQL",
                    Pattern = WholeWord(@"postgresql|postgres")
                },
                PositiveTests = Collection(
                    Test("PostgreSQL"),
                    Test("Postgres")
                ),
                NegativeTests = Collection(
                    Test("xPostgreSQL"),
                    Test("PostgreSQLx"),
                    Test("xPostgres"),
                    Test("Postgresx")
                )
            },
            /* 
            new Item {
                Keyword = new Keyword {
                    KeywordId = ,
                    Name = "",
                    Pattern = WholeWord(@"")
                },
                PositiveTests = Collection(
                    Test(""),
                    Test(""),
                    Test("")
                ),
                NegativeTests = Collection(
                    Test(""),
                    Test(""),
                    Test("")
                )
            }, 
            */
        };
    }
}