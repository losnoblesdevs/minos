﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Minos.Site.Models;

namespace Minos.Site.Controllers
{
    public class AdminController : Controller
    {
        private IProfessorRepository _professorRepository;

        public AdminController(
            IProfessorRepository professorRepository)
        {
            _professorRepository = professorRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CadastrarProfessor(string nome, string sobrenome, string serie, Grau grau)
        {

            Professor professor = new Professor(nome, sobrenome);
            Turma turma = new Turma(serie, grau);
            if (!professor.ValidaProfessor() || !turma.ValidaTurmas())
            {
                ViewData["Message"] = "Envie os dados do professor de forma correta!";
                return View();
            }
            else
            {
                _professorRepository.Salvar(professor);
            }
                  
            return View();
        }
    }
}