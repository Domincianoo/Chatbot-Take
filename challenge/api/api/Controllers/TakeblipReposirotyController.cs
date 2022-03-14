using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Controllers
{
    [ApiController]
    public class TakeblipReposirotyController : ControllerBase
    {

        private readonly ILogger<TakeblipReposirotyController> _logger;

        public TakeblipReposirotyController(ILogger<TakeblipReposirotyController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("/")]
        public async Task<string> Get()
        {
            List<TakeblipRepository> takeblipRepositories = new List<TakeblipRepository>();
            string tokenGithub = "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";
            
            //Utilizando o nuget Octokit, simplificaremos o acesso à API do Github, não sendo necessário: informar a URL da API, o método(Get, Post, Patch, etc...), alterar o cabeçalho do protocolo HTTP(para informar o modo de autenticação,por exemplo), criar as classes para receber o Json retornado da API.
            var github = new GitHubClient(new ProductHeaderValue("ChatbotAPI"));

            github.Credentials = new Credentials(tokenGithub);

            var allRepositories = await github.Repository.GetAllForUser("takenet");

            var csharpRepositories = allRepositories.Where(a => a.Language == "C#").OrderBy(x => x.CreatedAt).ToList();
            var olderCSharpRepositories = csharpRepositories.Take(5).ToList();

            olderCSharpRepositories.ForEach(x => takeblipRepositories.Add(new TakeblipRepository() { FullName = x.FullName, Description = x.Description }));

            string output = JsonConvert.SerializeObject(takeblipRepositories);

            return output;
        }
    }
}
