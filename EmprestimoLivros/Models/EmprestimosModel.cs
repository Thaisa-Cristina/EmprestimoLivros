using System.ComponentModel.DataAnnotations;

namespace EmprestimoLivros.Models
{
    public class EmprestimosModel
    {
        public int Id { get; set; }
        public string Recebedor { get; set; }
        [Required(ErrorMessage = "Digite o nome do Recebedor!")]
        public string Fornecedor { get; set; }
        [Required(ErrorMessage = "Digite o nome do Fornecedor!")]
        public string LivroEmprestado { get; set; }
        [Required(ErrorMessage = "Digite o nome do Livro!")]
        public DateTime DataEmprestimo { get; set; }

    }
}
