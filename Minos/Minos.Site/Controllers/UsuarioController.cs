﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Minos.Site.Models;

namespace Minos.Site.Controllers
{
    public class UsuarioController : Controller
    {
        private IUsuarioRepository _usuarioRepository;

        public UsuarioController(
            IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }


        public IActionResult Index()
        {
            if (!_usuarioRepository.Existe("admin"))
            {
                Usuario user = new Usuario(
                    login: "admin",
                    senha: "admin"
                    );
                    user.Admin = "S";

            _usuarioRepository.Salvar(user);

            }
            return RedirectToAction("Login");

        }

        [HttpGet]
        public IActionResult CadastrarUsuario()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CadastrarUsuario(string login, string senha, string repitaSenha)
        {
            Usuario usuario = new Usuario(login, senha);
            var mensagem = new Mensagem();

            if (_usuarioRepository.Existe(login))
            {
                ViewData["Message"] = "Usuario já existe!";
                ViewData["Status"] = "bg-danger";
                return View();
            }

            if (!usuario.ValidaLogin())
            {
                return View();
            }

            if (!usuario.ValidaSenha())
            {
                ViewData["Message"] = "Por favor Digite uma Senha Válida!";
                ViewData["Status"] = "bg-danger";
                ViewData["NomeUsuario"] = login;
                return View();
            }
            if (senha != repitaSenha)
            {
                ViewData["Message"] = "As senhas não são iguais!";
                ViewData["Status"] = "bg-warning";
                return View();
            }

            ViewData["Message"] = "Cadastro efetuado com sucesso!";
            ViewData["Status"] = "bg-success";
            _usuarioRepository.Salvar(usuario);

            return View();

        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string login, string senha)
        {
            Usuario usuario = new Usuario(login, senha);
            if (usuario.ValidaLogin() == false || usuario.ValidaSenha() == false)
            {
                ViewData["Message"] = "Por favor verifique se todas as informações foram preenchidas corretamente!";
                return View();
            }

            else
            {
                if (usuario.EhValido())
                {
                    if (_usuarioRepository.EhAdm(login, senha))
                    {
                        if (_usuarioRepository.DadosDeLoginSaoValidos(login, senha))
                        {
                            HttpContext.Session.SetString("LogarAdm", "Administrador");
                            return RedirectToAction("Index", "Admin");

                        }
                        else
                        {
                            ViewData["Message"] = "Por favor verifique se todas as informações foram preenchidas corretamente!";
                            return View();
                        }
                    }

                    if (_usuarioRepository.DadosDeLoginSaoValidos(login, senha))
                    {
                        HttpContext.Session.SetString("LogarAluno", "Aluno");
                        return RedirectToAction("Index", "Aluno");

                    }
                    else
                    {
                        ViewData["Message"] = "Por favor verifique se todas as informações foram preenchidas corretamente!";
                        return View();
                    }
                }
            }

            return View();
        }



        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("LogarAdm");
            HttpContext.Session.Remove("LogarAluno");
            return RedirectToAction("Login", "Usuario");
        }

    }
}