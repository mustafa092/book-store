using application.DTOs.infrastructure;
using application.Services.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class BooksController : BaseController
{
    private readonly ILogger<BooksController> _logger;

    private readonly IMediator _mediator;
    // adding mediator to the controller

    public BooksController(IMediator mediator, ILogger<BooksController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> LoadBooks([FromQuery] LoadingBooksRequest request)
    {
        try
        {
            _logger.LogInformation("Loading books");
            var books = await _mediator.Send(new LoadBooksQuery
            {
                SearchKey = request.SearchKey,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            });
            return Ok(books);
        }
        catch (Exception ex)
        {
            // _logger.LogError(ex, "Error loading books");
            _logger.LogError(ex, "Error loading books");
            return StatusCode(500, "Internal server error");
        }
    }
}