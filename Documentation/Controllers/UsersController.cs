using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using APILibrary.Models.Todos;
using APILibrary.Models.Users;
using APILibrary.Repositories;
using APILibrary.Repositories.Users;

namespace Documentation.Controllers
{
    //Controller route api/todos
    [ApiController]
    [Route("api/users")]
    public class TodosController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;

        public TodosController(IPostRepository todoRepository, IUserRepository userRepository)
        {
            _postRepository = todoRepository;
            _userRepository = userRepository;
        }


        [HttpGet]
        public IEnumerable<User> GetUsers()
        {
            return _userRepository.GetUsers();
        }


        [HttpGet]
        [Route("{id:int}")]
        public ActionResult<User> GetUser(int id)
        {
            var user = _userRepository.GetUserWithId(id);
            if (user is null)
                return NotFound(user);
            return user;
        }


        [HttpPost]
        public ActionResult<User> CreateUser(User user)
        {
                _userRepository.Add(user);
                return user;
        }

        /*
        [HttpPost]
        public ActionResult CreateUser(UserDto userDto)
        {
            try
            {
                var user = _userRepository.GetUser(userDto.CreatedBy);
                var todo = _postRepository.Add(userDto, user);
                return CreatedAtAction(nameof(GetUser), new { id = todo.Id }, todo);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }
        */

        //Ludvig, förklara för mig vad denna endpoint gör och hur den fungerar.
        //Om Postman skickar en request av typen Delete och med routen api/posts/1, så kommer
        //elementet med id 1 i min Dictionary att tas bort. 

        //Om jag klickar på send i Postman så tas ett inlägg i min dictionary i C# bort.
        //Jättesmart och smidigt ju.

        [HttpDelete]
        [Route("{id:int}")]
        public ActionResult DeleteUser(int id)
        {
            var user = _userRepository.GetUserWithId(id);
            if (user is null)
                return NotFound($"No user with {id} found");
            _userRepository.Delete(user);
            return NoContent();
        }

        /*
        private IEnumerable<Post> RunTodoQuery(PostQueryDto todoQueryDto)
        {
            if (todoQueryDto.IsEmpty)
                return _postRepository.GetPosts();
            else if (!(todoQueryDto.Completed is null))
                return _postRepository.GetPostsCreatedBy(todoQueryDto.CreatedBy);
            else
                throw new NotSupportedException("The query combination selected is not supported");
        }

        */
    }
}