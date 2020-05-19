using BoardGame.API.Controllers;
using BoardGame.API.Services;
using BoardGame.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace BoardGame.Tests
{
    [TestClass]
    public class BoardGameControllerTest
    {

        #region CreateBoard Tests

        [TestMethod]
        public async Task CreateBoard_ValidSize_ShouldReturnOk()
        {
            //Arrange
            var redisServiceMock = new Mock<IRedisService>();
            redisServiceMock.Setup(r => r.Set<It.IsAnyType>(It.IsAny<string>(), It.IsAny<It.IsAnyType>())).ReturnsAsync(true);
            var helperServiceMock = new Mock<IGameHelperService>();
            var controller = new BoardGameController(redisServiceMock.Object, helperServiceMock.Object);

            //Act
            IActionResult result =  await controller.CreateBoard(10);
            var okResult = result as OkObjectResult;

            //Asset
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

        }

        [TestMethod]
        public async Task CreateBoard_InValidSize_ZeroSize_ShouldReturnBadRequest()
        {
            //Arrange
            int size = 0;

            var redisServiceMock = new Mock<IRedisService>();
            redisServiceMock.Setup(r => r.Set<It.IsAnyType>(It.IsAny<string>(), It.IsAny<It.IsAnyType>())).ReturnsAsync(true);
            var helperServiceMock = new Mock<IGameHelperService>();
            var controller = new BoardGameController(redisServiceMock.Object, helperServiceMock.Object);

            //Act
            IActionResult result = await controller.CreateBoard(size);
            var badRequestResult = result as BadRequestObjectResult;

            //Asset
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            Assert.AreEqual("Board cannot be created of size " + size, badRequestResult.Value);

        }

        [TestMethod]
        public async Task CreateBoard_InValidSize_NegativeSize_ShouldReturnBadRequest()
        {
            //Arrange
            int size = -10;
            var redisServiceMock = new Mock<IRedisService>();
            redisServiceMock.Setup(r => r.Set<It.IsAnyType>(It.IsAny<string>(), It.IsAny<It.IsAnyType>())).ReturnsAsync(true);
            var helperServiceMock = new Mock<IGameHelperService>();
            var controller = new BoardGameController(redisServiceMock.Object, helperServiceMock.Object);

            //Act
            IActionResult result = await controller.CreateBoard(size);
            var badRequestResult = result as BadRequestObjectResult;

            //Asset
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            Assert.AreEqual("Board cannot be created of size " + size, badRequestResult.Value);

        }

        #endregion

        #region GetBoard Tests

        [TestMethod]
        public async Task GetBoard_ValidBoardId_ShouldReturnOk()
        {
            //Arrange
            var redisServiceMock = new Mock<IRedisService>();
            var board = new Board(10);
            redisServiceMock.Setup(r => r.Get<Board>(It.IsAny<string>())).ReturnsAsync(board);

            var helperServiceMock = new Mock<IGameHelperService>();
            var controller = new BoardGameController(redisServiceMock.Object, helperServiceMock.Object);

            //Act
            IActionResult result = await controller.GetBoard(board.BoardId);
            var okResult = result as OkObjectResult;

            //Asset
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsInstanceOfType(okResult.Value, typeof(Board));
            Assert.AreEqual(board.BoardId, ((Board)okResult.Value).BoardId);

        }

        [TestMethod]
        public async Task GetBoard_InValidBoardId_ShouldReturnNotFound()
        {
            //Arrange
            var redisServiceMock = new Mock<IRedisService>();
            
            redisServiceMock.Setup(r => r.Get<Board>(It.IsAny<string>())).ReturnsAsync(default(Board));

            var helperServiceMock = new Mock<IGameHelperService>();
            var controller = new BoardGameController(redisServiceMock.Object, helperServiceMock.Object);

            var invalidBoard = new Board(10);

            //Act
            IActionResult result = await controller.GetBoard(invalidBoard.BoardId);
            var notFoundResult = result as NotFoundObjectResult;

            //Asset
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
            Assert.AreEqual("Unable to get board for request " + invalidBoard.BoardId.ToString(), notFoundResult.Value);

        }
        #endregion

        #region AddBattleshipTests
        [TestMethod]
        public async Task AddBattleship_ValidShip_ShouldReturnOk()
        {
            //Arrange
            var board = new Board(10);
            var ship = new Battleship() { ShipId = 1, Size = 5, StartColumn = 0, StartRow = 0, IsHorizontalDirection = true };

            var redisServiceMock = new Mock<IRedisService>();
            redisServiceMock.Setup(r => r.Get<Board>(It.IsAny<string>())).ReturnsAsync(board);
            redisServiceMock.Setup(r => r.Set<It.IsAnyType>(It.IsAny<string>(), It.IsAny<It.IsAnyType>())).ReturnsAsync(true);

            var helperServiceMock = new Mock<IGameHelperService>();
            helperServiceMock.Setup(r => r.AreCellsAvailable(It.IsAny<Board>(), It.IsAny<Battleship>())).Returns(true);
            helperServiceMock.Setup(r => r.AddBattleShip(It.IsAny<Board>(), It.IsAny<Battleship>())).Returns(true);

            var controller = new BoardGameController(redisServiceMock.Object, helperServiceMock.Object);

            //Act
            IActionResult result = await controller.AddBattleship(board.BoardId,ship);
            var okResult = result as OkObjectResult;

            //Asset
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

        }

        [TestMethod]
        public async Task AddBattleship_InValidShip_Null_ShouldReturnBadRequest()
        {
            //Arrange
            var board = new Board(10);
            Battleship ship = null ;

            var redisServiceMock = new Mock<IRedisService>();
            
            var helperServiceMock = new Mock<IGameHelperService>();
           
            var controller = new BoardGameController(redisServiceMock.Object, helperServiceMock.Object);

            //Act
            IActionResult result = await controller.AddBattleship(board.BoardId, ship);
            var badRequestResult = result as BadRequestObjectResult;

            //Asset
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            Assert.AreEqual("Cannot add ship of empty size", badRequestResult.Value);

        }

        [TestMethod]
        public async Task AddBattleship_InValidShip_ZeroSize_ShouldReturnBadRequest()
        {
            //Arrange
            var board = new Board(10);
            var ship = new Battleship() { ShipId=1, Size=0, StartRow=0, StartColumn=0, IsHorizontalDirection=true };

            var redisServiceMock = new Mock<IRedisService>();

            var helperServiceMock = new Mock<IGameHelperService>();

            var controller = new BoardGameController(redisServiceMock.Object, helperServiceMock.Object);

            //Act
            IActionResult result = await controller.AddBattleship(board.BoardId, ship);
            var badRequestResult = result as BadRequestObjectResult;

            //Asset
            Assert.IsNotNull(result);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            Assert.AreEqual("Cannot add ship of empty size", badRequestResult.Value);
        }

        [TestMethod]
        public async Task AddBattleship_InValidBoard_ShouldReturnBadRequest()
        {
            //Arrange
            var board = new Board(10);
            var ship = new Battleship() { ShipId = 1, Size = 5, StartColumn = 0, StartRow = 0, IsHorizontalDirection = true };

            var redisServiceMock = new Mock<IRedisService>();
            redisServiceMock.Setup(r => r.Get<Board>(It.IsAny<string>())).ReturnsAsync(default(Board));
            
            var helperServiceMock = new Mock<IGameHelperService>();
            
            var controller = new BoardGameController(redisServiceMock.Object, helperServiceMock.Object);

            //Act
            IActionResult result = await controller.AddBattleship(board.BoardId, ship);
            var badRequestResult = result as BadRequestObjectResult;

            //Asset
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            Assert.AreEqual("Unable to get board for request " + board.BoardId.ToString(), badRequestResult.Value);

        }

        [TestMethod]
        public async Task AddBattleship_CellsNotAvailable_ShouldReturnBadRequest()
        {
            //Arrange
            var board = new Board(10);
            var ship = new Battleship() { ShipId = 1, Size = 5, StartColumn = 0, StartRow = 0, IsHorizontalDirection = true };

            var redisServiceMock = new Mock<IRedisService>();
            redisServiceMock.Setup(r => r.Get<Board>(It.IsAny<string>())).ReturnsAsync(board);

            var helperServiceMock = new Mock<IGameHelperService>();
            helperServiceMock.Setup(r => r.AreCellsAvailable(It.IsAny<Board>(), It.IsAny<Battleship>())).Returns(false);

            var controller = new BoardGameController(redisServiceMock.Object, helperServiceMock.Object);

            //Act
            IActionResult result = await controller.AddBattleship(board.BoardId, ship);
            var badRequestResult = result as BadRequestObjectResult;

            //Asset
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            Assert.AreEqual("Selected position is not available on board " + board.BoardId.ToString(), badRequestResult.Value);

        }
        #endregion

        #region AttackResultTests
        [TestMethod]
        public async Task AttackResult_ValidPosition_ShouldReturnOk()
        {
            //Arrange
            var board = new Board(10);
            int row = 0;
            int column = 0;


            var redisServiceMock = new Mock<IRedisService>();
            redisServiceMock.Setup(r => r.Get<Board>(It.IsAny<string>())).ReturnsAsync(board);
            redisServiceMock.Setup(r => r.Set<It.IsAnyType>(It.IsAny<string>(), It.IsAny<It.IsAnyType>())).ReturnsAsync(true);

            var helperServiceMock = new Mock<IGameHelperService>();
            helperServiceMock.Setup(r => r.IsAttacSuccessful(It.IsAny<Board>(), It.IsAny<int>(), It.IsAny<int>())).Returns(true);
           
            var controller = new BoardGameController(redisServiceMock.Object, helperServiceMock.Object);

            //Act
            IActionResult result = await controller.AttackResult(board.BoardId, row,column);
            var okResult = result as OkObjectResult;

            //Asset
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

        }

        [TestMethod]
        public async Task AttackResult_InValidBoardId_ShouldReturnBadRequest()
        {
            //Arrange
            var board = new Board(10);
            int row = 0;
            int column = 0;


            var redisServiceMock = new Mock<IRedisService>();
            redisServiceMock.Setup(r => r.Get<Board>(It.IsAny<string>())).ReturnsAsync(default(Board));
            redisServiceMock.Setup(r => r.Set<It.IsAnyType>(It.IsAny<string>(), It.IsAny<It.IsAnyType>())).ReturnsAsync(true);

            var helperServiceMock = new Mock<IGameHelperService>();
           
            var controller = new BoardGameController(redisServiceMock.Object, helperServiceMock.Object);

            //Act
            IActionResult result = await controller.AttackResult(board.BoardId, row, column);
            var badRequestResult = result as BadRequestObjectResult;

            //Asset
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            Assert.AreEqual("Unable to get board for request " + board.BoardId.ToString(), badRequestResult.Value);

        }
        #endregion
    }
}
