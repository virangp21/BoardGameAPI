using BoardGame.API.Controllers;
using BoardGame.API.Services;
using BoardGame.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace BoardGame.Tests
{
    [TestClass]
    public class GameHelperServiceTest
    {
        #region AreCellsAvailable Tests

        [TestMethod]
        public void AreCellsAvailable_ValidShipSize_EmptyBoard_ShouldReturnTrue()
        {
            //Arrange
            var board = new Board(10);
            var ship = new Battleship() { ShipId = 1, Size = 5, StartColumn = 0, StartRow = 0, IsHorizontalDirection = true };
            var helperService = new GameHelperService();

            //Act
            var result = helperService.AreCellsAvailable(board, ship);

            //Asset
            Assert.IsTrue(result);

        }

        [TestMethod]
        public void AreCellsAvailable_InValidShipSize_EmptyBoard_ShouldReturnFalse()
        {
            //Arrange
            var board = new Board(10);
            var ship = new Battleship() { ShipId = 1, Size = 15, StartColumn = 0, StartRow = 0, IsHorizontalDirection = true };
            var helperService = new GameHelperService();

            //Act
            var result = helperService.AreCellsAvailable(board, ship);

            //Asset
            Assert.IsFalse(result);

        }

        [TestMethod]
        public void AreCellsAvailable_ValidShipSize_BoardOccupied_ShouldReturnFalse()
        {
            //Arrange
            var board = new Board(10);
            board.Cells.ForEach(x => x.IsOccupied = true);

            var ship = new Battleship() { ShipId = 1, Size = 5, StartColumn = 0, StartRow = 0, IsHorizontalDirection = true };
            var helperService = new GameHelperService();

            //Act
            var result = helperService.AreCellsAvailable(board, ship);

            //Asset
            Assert.IsFalse(result);

        }

        [TestMethod]
        public void AreCellsAvailable_NullBoard_ShouldReturnFalse()
        {
            //Arrange
            var board = default(Board);
            var ship = new Battleship() { ShipId = 1, Size = 5, StartColumn = 0, StartRow = 0, IsHorizontalDirection = true };
            var helperService = new GameHelperService();

            //Act
            var result = helperService.AreCellsAvailable(board, ship);

            //Asset
            Assert.IsFalse(result);

        }

        [TestMethod]
        public void AreCellsAvailable_NullShip_ShouldReturnFalse()
        {
            //Arrange
            var board = new Board(10);
            var ship = default(Battleship);
            var helperService = new GameHelperService();

            //Act
            var result = helperService.AreCellsAvailable(board, ship);

            //Asset
            Assert.IsFalse(result);

        }

        #endregion

        #region AddBattleShip Tests
        [TestMethod]
        public void AddBattleShip_ValidShip_EmptyBoard_ShouldReturnTrue()
        {
            //Arrange
            var board = new Board(10);
            var ship = new Battleship() { ShipId = 1, Size = 5, StartColumn = 0, StartRow = 0, IsHorizontalDirection = true };
            var helperService = new GameHelperService();

            //Act
            var result = helperService.AddBattleShip(board, ship);
            var occupiedCells = board.Cells.Where(x => x.IsOccupied == true).ToList();
            //Asset
            Assert.IsTrue(result);
            Assert.AreEqual(ship.Size, occupiedCells.Count);

        }

        [TestMethod]
        public void AddBattleShip_NullShip_ShouldReturnFalse()
        {
            //Arrange
            var board = new Board(10);
            var ship = default(Battleship);
            var helperService = new GameHelperService();

            //Act
            var result = helperService.AddBattleShip(board, ship);
            
            //Asset
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void AddBattleShip_NullBoard_ShouldReturnFalse()
        {
            //Arrange
            var board = default(Board);
            var ship = new Battleship() { ShipId = 1, Size = 5, StartColumn = 0, StartRow = 0, IsHorizontalDirection = true };
            var helperService = new GameHelperService();

            //Act
            var result = helperService.AddBattleShip(board, ship);

            //Asset
            Assert.IsFalse(result);
        }
        #endregion

        #region IsAttacSuccessful Tests
        [TestMethod]
        public void IsAttacSuccessful_ValidBoard_ValidRow_Column_ShouldReturnTrue()
        {
            //Arrange
            var board = new Board(10);
            board.Cells.Where(x => x.Row == 0 && x.Column == 0).First().IsOccupied = true;

            int row = 0;
            int column = 0;
            var helperService = new GameHelperService();

            //Act
            var result = helperService.IsAttacSuccessful(board, row,column);
            var hitCells = board.Cells.Where(x => x.IsHit == true).ToList();

            //Asset
            Assert.IsTrue(result);
            Assert.AreEqual(1, hitCells.Count);

        }

        [TestMethod]
        public void IsAttacSuccessful_ValidBoard_ValidRow_NotOccupied_ShouldReturnFalse()
        {
            //Arrange
            var board = new Board(10);
            board.Cells.Where(x => x.Row == 0 && x.Column == 0).First().IsOccupied = true;

            int row = 1;
            int column = 0;
            var helperService = new GameHelperService();

            //Act
            var result = helperService.IsAttacSuccessful(board, row, column);
            var hitCells = board.Cells.Where(x => x.IsHit == true).ToList();

            //Asset
            Assert.IsFalse(result);
            Assert.AreEqual(0, hitCells.Count);

        }

        [TestMethod]
        public void IsAttacSuccessful_ValidBoard_ValidColumn_NotOccupied_ShouldReturnFalse()
        {
            //Arrange
            var board = new Board(10);
            board.Cells.Where(x => x.Row == 0 && x.Column == 0).First().IsOccupied = true;

            int row = 0;
            int column = 1;
            var helperService = new GameHelperService();

            //Act
            var result = helperService.IsAttacSuccessful(board, row, column);
            var hitCells = board.Cells.Where(x => x.IsHit == true).ToList();

            //Asset
            Assert.IsFalse(result);
            Assert.AreEqual(0, hitCells.Count);

        }

        [TestMethod]
        public void IsAttacSuccessful_ValidBoard_InValidRow_OutsideBoard_ShouldReturnFalse()
        {
            //Arrange
            var board = new Board(10);
            board.Cells.Where(x => x.Row == 0 && x.Column == 0).First().IsOccupied = true;

            int row = 99;
            int column = 0;
            var helperService = new GameHelperService();

            //Act
            var result = helperService.IsAttacSuccessful(board, row, column);
            var hitCells = board.Cells.Where(x => x.IsHit == true).ToList();

            //Asset
            Assert.IsFalse(result);
            Assert.AreEqual(0, hitCells.Count);

        }

        [TestMethod]
        public void IsAttacSuccessful_ValidBoard_InValidColumn_OutsideBoard_ShouldReturnFalse()
        {
            //Arrange
            var board = new Board(10);
            board.Cells.Where(x => x.Row == 0 && x.Column == 0).First().IsOccupied = true;

            int row = 0;
            int column = 99;
            var helperService = new GameHelperService();

            //Act
            var result = helperService.IsAttacSuccessful(board, row, column);
            var hitCells = board.Cells.Where(x => x.IsHit == true).ToList();

            //Asset
            Assert.IsFalse(result);
            Assert.AreEqual(0, hitCells.Count);

        }
        #endregion
    }
}
