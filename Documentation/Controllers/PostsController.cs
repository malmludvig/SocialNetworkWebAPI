using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using APILibrary.Models.Todos;
using APILibrary.Repositories;
using APILibrary.Repositories.Users;

namespace Documentation.Controllers
{
    //Controller route api/todos
    [ApiController]
    [Route("api/posts")]
    public class PostsController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;

        public PostsController(IPostRepository todoRepository, IUserRepository userRepository)
        {
            _postRepository = todoRepository;
            _userRepository = userRepository;
        }

        /*
        [HttpGet]
        public IEnumerable<Todo> GetAllQueried([FromQuery] TodoQueryDto todoQuery)
        {
            return RunTodoQuery(todoQuery);
        }
        */

        [HttpGet]
        public IEnumerable<Post> GetPosts()
        {
            return _postRepository.GetPosts();
        }


        [HttpGet]
        [Route("{id:int}")]
        public ActionResult<Post> GetPost(int id)
        {
            var post = _postRepository.GetPostWithId(id);
            if (post is null)
                return NotFound(post);
            return post;
        }
                                                                        
        [HttpPost]
        public ActionResult CreatePost(PostDto postDto)
        {
            try
            {
                var user = _userRepository.GetUser(postDto.CreatedBy);
                var post = _postRepository.Add(postDto, user);
                return CreatedAtAction(nameof(GetPost), new { id = post.Id }, post);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }

        
        [HttpPut]
        [Route("{id:int}")]
        public ActionResult ReplacePost(int id, PostDto postPut)
        {
            var user = _userRepository.GetUser(id);
            var todo = _postRepository.GetPostWithId(id);
            if (todo is null)
                return NotFound($"No todo with {id} found");
            var putPost = new Post(id, postPut, user);
            _postRepository.Update(putPost);
            return NoContent();
        }

        [HttpPut]
        [Route("likes/{id:int}")]
        public ActionResult AddALikeToAPost(int id, PostLikeDto postPut)
        {
            var user = _userRepository.GetUser(id);
            var todo = _postRepository.GetPostWithId(id);
            if (todo is null)
                return NotFound($"No todo with {id} found");
            var putPost = new Post(id, postPut, user);
            _postRepository.Update(putPost);
            return NoContent();
        }


        [HttpPatch]
        [Route("{id:int}")]
        public ActionResult UpdatePost(int id, Dictionary<string, object> patches)
        {
            var todo = _postRepository.GetPostWithId(id);
            if (todo is null)
                return NotFound($"No todo with {id} found");
            _postRepository.ApplyPatch(todo, patches);
            return NoContent();
        }

        [HttpDelete]
        [Route("{id:int}")]
        public ActionResult DeletePost(int id)
        {
            var todo = _postRepository.GetPostWithId(id);
            if (todo is null)
                return NotFound($"No todo with {id} found");
            _postRepository.Delete(todo);
            return NoContent();
        }

        private IEnumerable<Post> PostTodoQuery(PostQueryDto postQueryDto)
        {
            if (postQueryDto.IsEmpty)
                return _postRepository.GetPosts();
            else if (!(postQueryDto.Completed is null))
                return _postRepository.GetPostsCreatedBy(postQueryDto.CreatedBy);
            else
                throw new NotSupportedException("The query combination selected is not supported");
        }
    }
}