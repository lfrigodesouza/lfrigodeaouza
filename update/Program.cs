using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;

var template = @"# ✌ Olá, seja bem-vindo(a)!

Me chamo **Lucas**, sou formado em Engenharia da Computação, tenho MBA em Engenharia de Software e estou cursando um MBA em CyberSecurity.
Com 10 anos de experiência em TI, trabalho atualmente como desenvolvedor sênior.

Meu foco é desenvolvimento backend com .NET e em DevSec, mas sempre busco novos conhecimentos nos mais diversos assuntos e tecnologias, e nos meus repositórios você vai encontrar todos tipos de projetos.
</br><p align=""center"">
<a href=""https://www.linkedin.com/in/lfrigodesouza/""><img src=""https://img.shields.io/badge/-LinkedIn-0077B5?style=flat-square&logo=Linkedin&logoColor=white&link=https://www.linkedin.com/in/lfrigodesouza/""></a>
<a href=""https://twitter.com/lfrigodesouza/""><img src=""https://img.shields.io/badge/-Twitter-1DA1F2?style=flat-square&logo=twitter&logoColor=white&link=https://twitter.com/lfrigodesouza/""></a>
<a href=""https://LFrigoDeSouza.NET/""><img src=""https://img.shields.io/badge/-LFS.NET-9e9e9e?style=flat-square&logo=microsoft-edge&logoColor=white&link=https://LFrigoDeSouza.NET/""></a>
</p>

## ✒️Artigos Recentes
<ul>
#ARTICLES_PLACEHOLDER#
<li style=""list-style-type: none;""><a href=""https://blog.lfrigodesouza.net"" target=""_blank"">Veja mais...</a></li>
</ul>

## 👨‍💻 Meu GitHub
<p align=""center"">
<img src=""https://github-readme-stats.vercel.app/api/top-langs/?username=lfrigodesouza&layout=compact&theme=dark""/>
<img src=""https://github-readme-stats.vercel.app/api?username=lfrigodesouza&show_icons=true&theme=dark"">
</p>
";
Console.WriteLine("Iniciando processamento do README.md");
var url = "https://lfrigodesouza-functions.azurewebsites.net/api/blog-latests-posts";
var client = new HttpClient();
var response = await client.GetAsync(url);
if (!response.IsSuccessStatusCode)
    throw new Exception("Não foi possível obter os dados");
var responseContent = await response.Content.ReadAsStringAsync();
if (string.IsNullOrWhiteSpace(responseContent))
    throw new Exception("Não foi possível obter os dados");
var posts = JsonDocument.Parse(responseContent).RootElement;
var postsLinks = new StringBuilder();
for (var i = 0; i < 5; i++)
{
    var (title, postDate, permaLink) = GetPostDetails(posts[i]);

    postsLinks.AppendLine(
        $"<li style=\"list-style-type: none;\"><a href=\"{permaLink}\" target=\"_blank\">{title}</a><i> &nbsp;({GetPublishDate(postDate)})</i></li>");
    // postsLinks.AppendLine($"* [{title}]({permaLink}) _({GetPublishDate(postDate)})_ ");
}

var readmeContent = template.Replace("#ARTICLES_PLACEHOLDER#", postsLinks.ToString());
File.WriteAllText("../README.md", readmeContent);

string GetPublishDate(DateTime postDate)
{
    var daysFromPost = (DateTime.Now.Date - postDate.Date).TotalDays;
    return daysFromPost switch
    {
        < 1 => "hoje",
        < 2 => "1 dia atrás",
        _ => $"{daysFromPost.ToString("#")} dias atrás"
    };
}

(string title, DateTime postDate, string permaLink) GetPostDetails(JsonElement post)
{
    var title = post.GetProperty("title").ToString();
    var postDate = post.GetProperty("date").GetDateTime();
    var permaLink = post.GetProperty("permalink").ToString();
    return (title, postDate, permaLink);
}
