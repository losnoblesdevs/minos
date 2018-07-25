﻿using Minos.Site.Controllers;
using Minos.Site.Models;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Minos.UnitTests
{
    public class ProfessorTests
    {
        private Mock<IProfessorRepository> professorRepositoryMock;
        private Mock<ITurmaRepository> turmaRepositoryMock;
        private AdminController sut;
        private Mock<Turma> turmaMock;
        public List<int> turmaId;
        private Turma turmaVazia;

        public void PopulaTurmaId()
        {
            turmaId = new List<int>();
            turmaId.Add(1);
        }

        public void CriaMock()
        {
            this.professorRepositoryMock = new Mock<IProfessorRepository>();
            this.turmaRepositoryMock = new Mock<ITurmaRepository>();
        }

        
        public void CriaAdminController()
        {
            this.sut = new AdminController(professorRepositoryMock.Object, turmaRepositoryMock.Object);
            
            
        }

        public void CriaTurmaMock()
        {
            this.turmaMock = new Mock<Turma>();
        }

        [Trait("ProfessorController", "Salvar Professor")]
        [Fact(DisplayName = "Deveria Salvar Professor Chamando Repository Uma Vez")]
        public void DeveriaSalvarProfessorChamandoRepositoryUmaVez()
        {
            //arrange
            CriaMock();
            CriaTurmaMock();
            PopulaTurmaId();
            
            //act
            CriaAdminController();
            turmaRepositoryMock.Setup(x => x.ObterTurmaPeloId(It.IsAny<int>())).Returns(new Turma(Serie.Nono, Grau.Medio));
            sut.CadastrarProfessor("Robson", "Junior", turmaId);
            

            //assert
            professorRepositoryMock.Verify(x => x.Salvar(It.IsAny<Professor>()), Times.Once);
            
        }



        [Trait("ProfessorController", "Deveria Não Salvar Nome Do Professor Com Numeros")]
        [Fact(DisplayName = "Deveria Não Salvar Nome Do Professor Com Numeros")]
        public void DeveriaNaoSalvarProfessorComNumero()
        {
            //arrange
            CriaMock();
            PopulaTurmaId();

            //act
            CriaAdminController();
            turmaRepositoryMock.Setup(x => x.ObterTurmaPeloId(It.IsAny<int>())).Returns(new Turma(Serie.Nono, Grau.Medio));
            sut.CadastrarProfessor("Robso2n", "Robson", turmaId);


            //assert
            professorRepositoryMock.Verify(x => x.Salvar(It.IsAny<Professor>()), Times.Never);
        }

        [Trait("ProfessorController", "Deveria Não Salvar Com Nome Do Professor Null")]
        [Fact(DisplayName = "Deveria Não Salvar Com Nome Do Professor Null")]
        public void DeveriaNaoSalvarProfessorComNull()
        {
            //arrange
            CriaMock();
            PopulaTurmaId();

            //act
            CriaAdminController();
            turmaRepositoryMock.Setup(x => x.ObterTurmaPeloId(It.IsAny<int>())).Returns(new Turma(Serie.Nono, Grau.Medio));
            sut.CadastrarProfessor("Robson", null, turmaId);

            //assert
            professorRepositoryMock.Verify(x => x.Salvar(It.IsAny<Professor>()), Times.Never);

        }

        [Trait("ProfessorController", "Deveria Não Salvar Com Nome Do Professor Vazio")]
        [Fact(DisplayName = "Deveria Não Salvar Com Nome Do Professor Vazio")]
        public void DeveriaNaoSalvarProfessorComVazio()
        {
            //arrange
            CriaMock();
            PopulaTurmaId();

            //act
            CriaAdminController();
            turmaRepositoryMock.Setup(x => x.ObterTurmaPeloId(It.IsAny<int>())).Returns(new Turma(Serie.Nono, Grau.Medio));
            sut.CadastrarProfessor("", "Junior", turmaId);

            //assert
            professorRepositoryMock.Verify(x => x.Salvar(It.IsAny<Professor>()), Times.Never);

        }

        [Trait("ProfessorController", "Deveria Não Salvar Com a conexão com o repository retornando null")]
        [Fact(DisplayName = "Deveria Não Salvar Com a conexão com o Repository De Turma Retornando Null")]
        public void DeveriaNaoSalvarComAConexaoComRepositoryDeTurmaRetornandoNull()
        {
            //arrange
            CriaMock();
            PopulaTurmaId();

            //act
            CriaAdminController();
            turmaRepositoryMock.Setup(x => x.ObterTurmaPeloId(It.IsAny<int>())).Returns(turmaVazia);
            sut.CadastrarProfessor("Robson", "Junior", turmaId);

            //assert
            professorRepositoryMock.Verify(x => x.Salvar(It.IsAny<Professor>()), Times.Never);

        } 
    }
}