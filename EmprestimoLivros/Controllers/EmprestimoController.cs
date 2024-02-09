using ClosedXML.Excel;
using EmprestimoLivros.Data;
using EmprestimoLivros.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;


namespace EmprestimosBase.Controllers
{
    public class EmprestimoController : Controller
    {
        readonly private ApplicationDbContext _db;

        public EmprestimoController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<EmprestimosModel> emprestimos = _db.Emprestimos;
            return View(emprestimos);
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Editar(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            EmprestimosModel emprestimo = _db.Emprestimos.FirstOrDefault(x => x.Id == id);

            if (emprestimo == null)
            {
                return NotFound();
            }

            return View(emprestimo);
        }

        [HttpGet]
        public IActionResult Excluir(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            EmprestimosModel emprestimo = _db.Emprestimos.FirstOrDefault(x => x.Id == id);

            if (emprestimo == null)
            {
                return NotFound();
            }

            return View(emprestimo);
        }

        [HttpPost]
        public IActionResult Cadastrar(EmprestimosModel emprestimos)
        {
            if (ModelState.IsValid)
            {
                _db.Emprestimos.Add(emprestimos);
                _db.SaveChanges();

                TempData["MensagemSucesso"] = "Cadastro realizado com sucesso!";

                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Editar(EmprestimosModel emprestimos)
        {
            if (ModelState.IsValid)
            {
                var emprestimoDB = _db.Emprestimos.Find(emprestimos.Id);

                emprestimoDB.LivroEmprestado = emprestimos.LivroEmprestado;
                emprestimoDB.Recebedor = emprestimos.Recebedor;
                emprestimoDB.Fornecedor = emprestimos.Fornecedor;

                _db.Emprestimos.Update(emprestimoDB);
                _db.SaveChanges();

                TempData["MensagemSucesso"] = "Edição realizado com sucesso!";

                return RedirectToAction("Index");
            }

            TempData["MensagemErro"] = "Ocorreu algum erro no momento da edição!";
            return View(emprestimos);
        }

        [HttpPost]
        public IActionResult Excluir(EmprestimosModel emprestimos)
        {
            if (emprestimos == null)
            {
                return NotFound();
            }

            _db.Emprestimos.Remove(emprestimos);
            _db.SaveChanges();

            TempData["MensagemSucesso"] = "Exclusão realizado com sucesso!";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Exportar()
        {
            var dados = GetDados();

            using (XLWorkbook workBook = new XLWorkbook())
            {
                workBook.AddWorksheet(dados, "Dados Empréstimos");
                using (MemoryStream ms = new MemoryStream())
                {
                    workBook.SaveAs(ms);
                    return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Emprestimo.xls");
                }

            }
        }

        private DataTable GetDados()
        {
            DataTable dataTable = new DataTable();

            dataTable.TableName = "Dados emprestimos";

            dataTable.Columns.Add("Recebedor", typeof(string));
            dataTable.Columns.Add("Fornecedor", typeof(string));
            dataTable.Columns.Add("Livro", typeof(string));
            dataTable.Columns.Add("Data última atualização", typeof(DateTime));

            var dados = _db.Emprestimos.ToList();

            if (dados.Count > 0)
            {
                dados.ForEach(emprestimo =>
                {
                    dataTable.Rows.Add(emprestimo.Recebedor, emprestimo.Fornecedor, emprestimo.LivroEmprestado, emprestimo.DataEmprestimo);
                });
            }

            return dataTable;

        }
    }
}