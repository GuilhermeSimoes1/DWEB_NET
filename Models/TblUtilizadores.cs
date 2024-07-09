using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace DWEB_NET.Models
{
    public class TblUtilizadores
    {

        public TblUtilizadores()
        {
            ListaContas = new HashSet<TblContas>();
            ListaTransacoes = new HashSet<TblTransacoes>();
            ListaOrcamentos = new HashSet<TblOrcamentos>();
            IsAdmin = false;
        }


        [Key]
        public int UserID { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório.")]
        public string UserName { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório.")]
        public string Email { get; set; }

        //[RegularExpression("[0-9]{1,6}([,.][0-9]{1,2})?", ErrorMessage = "Escreva um número com, no máximo 2 casas decimais, separadas por . ou ,")]
        [RegularExpression("[]")]
        public string? FirstName { get; set; }

        [StringLength(30)]
        public string? LastName { get; set; }

        [StringLength(255)]
        public string? Descricao { get; set; }

        public Boolean IsAdmin { get; set; } 


        /// <summary>
        /// atributo para funcionar como FK entre a tabela dos Utilizadores
        /// e a tabela da Autenticação
        /// </summary>
        public string UserAutent { get; set; }


        // relacionamento 1-N

        /// <summary>
        /// Lista das contas 
        /// </summary>
        public ICollection<TblContas> ListaContas { get; set; }

        // relacionamento 1-N

        /// <summary>
        /// Lista das Transacoes 
        /// </summary>

        public ICollection<TblTransacoes> ListaTransacoes { get; set; }

        // relacionamento 1-N

        /// <summary>
        /// Lista dos orçamentos 
        /// </summary>
        public ICollection<TblOrcamentos> ListaOrcamentos { get; set; }
    }
}
