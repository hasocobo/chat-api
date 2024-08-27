using ChatAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChatAPI.Presentation.Controllers;


[Route("api/v1/[controller]")]
public class ChatController : Controller 
{
  private readonly IChatService _chatService;

  public ChatController(IChatService chatService)
  {
    _chatService = chatService;
  }
  
  [HttpGet("messages")]
  public async Task<IActionResult> GetMessages()
  {
    var messages = _chatService.GetAllMessages();
    
    return Ok(messages);
  }
}
