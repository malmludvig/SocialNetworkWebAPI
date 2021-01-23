using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace APILibrary.Models.Todos
{
    public class PostLikeDto
    {
        private const string _stringMessage = "{0} must be between {2} and {1} characters long";

        [StringLength(400, ErrorMessage = _stringMessage, MinimumLength = 5)]
        public string Text { get; set; }

        public int CreatedBy { get; set; }
        public bool HasBeenEdited { get; set; }

        public List<int> ListOfUsersThatLikedThisPost { get; set; }

    }
}