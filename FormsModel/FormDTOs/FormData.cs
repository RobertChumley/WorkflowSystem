using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormsModel.FormDTOs
{
    public class FormData
    {

    }

    public class Form
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string PublicIdentifier { get; set; }
    }



}
