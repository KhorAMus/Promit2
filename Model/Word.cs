using System;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Word
    {
        [Key]

        public int Id { get; set; }
        public string Value { get; set; }
        public int Count { get; set; }
    }
}
