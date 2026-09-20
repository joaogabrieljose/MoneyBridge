using CustomerService.Application.DTOs.Requests;
using CustomerService.Application.Services;
using CustomerService.Domain.Modals;
using Microsoft.AspNetCore.Mvc;

namespace CustomerService.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly ICustumerRepository _repository;

    public CustomerController(ICustomerService customerService, ICustumerRepository repository)
    {
        _customerService = customerService;
        _repository = repository;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateCustomerRequest request)
    {
        var customer = await _customerService.CreateAsync(request);
        return StatusCode(StatusCodes.Status201Created, customer);
    }


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var customer = await _customerService.GetAllAsync();
        return Ok(customer);
    }


    [HttpGet]
    [Route("/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var customer = await _customerService.GetByIdAsync(id);

        if (customer is null)
        {
            return NotFound();
        }
        return Ok(customer);
    }

    [HttpPut]
    [Route("/{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCustomerRequest request)
    {
        var customer = await _customerService.UpdateAsync(id, request);
        return Ok(customer);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        await _customerService.DeleteAsync(id);
        return NoContent();
    }
    
    // paginação 
    [HttpGet]
    [Route("/page")]
    public IActionResult GetPage(int pageNumber, int pageQuantity)
    {
        var Customer = _repository.getPaginatedResult(pageNumber, pageQuantity);
        return Ok(Customer);
    }


}