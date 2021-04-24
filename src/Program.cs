using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;

var template = @"# Olá, sou o Lucas Frigo de Souza ✌

Sou formado em Engenharia da Computação, MBA em Engenharia de Software e cursando MBA em CyberSecurity e Ethical Hacking.
Com 10 anos de experiência em TI, trabalho atualmente como desenvolvedor fullstack principalmente com C# e ReactJS. 

Busco sempre novos conhecimentos nos mais diversos assuntos e tecnologias.

Não deixe de conferir os últimos artigos que escrevi para o meu blog:
#ARTICLES_PLACEHOLDER#


<br/><p align=""center"">
<a href=""https://www.linkedin.com/in/lfrigodesouza/""><img src=""https://img.shields.io/badge/-LinkedIn-0077B5?style=flat-square&logo=Linkedin&logoColor=white&link=https://www.linkedin.com/in/lfrigodesouza/""></a>
<a href=""https://twitter.com/lfrigodesouza/""><img src=""https://img.shields.io/badge/-Twitter-1DA1F2?style=flat-square&logo=twitter&logoColor=white&link=https://twitter.com/lfrigodesouza/""></a>
<a href=""https://LFrigoDeSouza.NET/""><img src=""https://img.shields.io/badge/-LFS.NET-9e9e9e?style=flat-square&logo=microsoft-edge&logoColor=white&link=https://LFrigoDeSouza.NET/""></a>
</p>
";

Console.WriteLine("Iniciando processamento do README.md");
var url = "https://blog.lfrigodesouza.net/content.json";
var client = new HttpClient();

var response = await client.GetAsync(url);
if(!response.IsSuccessStatusCode)
    throw new Exception("Não foi possível obter os dados");

var responseContent = await response.Content.ReadAsStringAsync();
if(string.IsNullOrWhiteSpace(responseContent))
    throw new Exception("Não foi possível obter os dados");

var content = JsonSerializer.Deserialize<Content>(responseContent);
var postsLinks = new StringBuilder();

for (int i = 0; i < 5; i++)
{
    postsLinks.AppendLine($"* [{content.posts[i].title}]({content.posts[i].permalink}) _({GetPublishDate(content.posts[i].date)})_ ");
}
var readmeContent = template.Replace("#ARTICLES_PLACEHOLDER#", postsLinks.ToString());
File.WriteAllText("../README.md", readmeContent);

string GetPublishDate(DateTime postDate){
    var daysFromPost = (DateTime.Now - postDate).TotalDays;
   return daysFromPost <= 0 ? "1 dia atrás" : $"{daysFromPost.ToString("#")} dias atrás";
}

    public class Meta
    {
        public string title { get; set; }
        public string subtitle { get; set; }
        public string description { get; set; }
        public string author { get; set; }
        public string url { get; set; }
        public string root { get; set; }
    }

    public class Page
    {
        public string title { get; set; }
        public DateTime date { get; set; }
        public DateTime updated { get; set; }
        public bool comments { get; set; }
        public string path { get; set; }
        public string permalink { get; set; }
        public string excerpt { get; set; }
        public string text { get; set; }
    }

    public class Category
    {
        public string name { get; set; }
        public string slug { get; set; }
        public string permalink { get; set; }
    }

    public class Tag
    {
        public string name { get; set; }
        public string slug { get; set; }
        public string permalink { get; set; }
    }

    public class Post
    {
        public string title { get; set; }
        public string slug { get; set; }
        public DateTime date { get; set; }
        public DateTime updated { get; set; }
        public bool comments { get; set; }
        public string path { get; set; }
        public string link { get; set; }
        public string permalink { get; set; }
        public string excerpt { get; set; }
        public string text { get; set; }
        public List<Category> categories { get; set; }
        public List<Tag> tags { get; set; }
    }

    public class Content
    {
        public Meta meta { get; set; }
        public List<Page> pages { get; set; }
        public List<Post> posts { get; set; }
        public List<Category> categories { get; set; }
        public List<Tag> tags { get; set; }
    }

