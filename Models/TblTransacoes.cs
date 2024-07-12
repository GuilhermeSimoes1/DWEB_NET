    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace DWEB_NET.Models
    {
        public class TblTransacoes
        {
            public TblTransacoes()
            {
                ListaTransacoesCategorias = new HashSet<TblTransacoesCategorias>();
            }

            public enum Tipo
            {
                Ganho,
                Gasto
            }

            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //Incrementa automaticamente
            public int TransacaoID { get; set; }

            [Display(Name = "Data")] 
            public DateTime DataTime { get; set; }

            [Display(Name = "Tipo de Transação")]
            public Tipo TipoTransacao { get; set; }         

            [StringLength(100)]
            [Display(Name = "Descrição")]
            public string? Descricao { get; set; }

            [Display(Name = "Valor da Transação")]
            public required decimal ValorTransacao { get; set; }

            [ForeignKey(nameof(Conta))]
            [Display(Name = "Conta associada")]
            public int ContaFK { get; set; }
            public TblContas Conta { get; set; }



            [ForeignKey(nameof(User))]
            [Display(Name = "Utilizador associado")]
            public int UserFK { get; set; }
            public TblUtilizadores User { get; set; }

            public ICollection<TblTransacoesCategorias> ListaTransacoesCategorias { get; set; }
        }


    }
