using Business.Abstract;
using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    //CONTROLLER BÖLÜMÜ KULLANICIDAN BİZE GELEN İSTEKLERİ KARŞILADIĞIMIZ YERDİR
    //YANİ HANGİ İSTEK GELECEKSE BİZİM O İSTEĞİ BURAYA YAZMAMIZ GEREKİR.
    [Route("api/[controller]")]
    [ApiController]
    //buradaki api controller ve aşağıdaki controllerbase => attribute tür. yani bir imzalamadır.
    //bu class bir contoller dır diyoruz yani.

    //ProductsController demek: c# yapımcıları bunu böyle yapmış. aslında
    // Controller kelimesinin önündeki kelimeyi request etmiş oluyoruz.
    //bizim amacımız da bur
    public class ProductsController : ControllerBase
    {
        //codemuzu refactoring edelim. consructor injection yapacağız önce.
        //fakat bu şekilde de Bizim controllerimiz. IProductService yi tanımaz.!
        //bunun çözümü de : IoC dir. yani Inversion of Control dür. Conteiner.
        //yani ben o conteiner içerisinde isteklerimizi new leyeyim. sen ne istsern gel bana oradan sor. oradan veririm.
        IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        //buraya kadar injection yaptık.Refactoring.
        //ÖNEMLİ somut yani concrete asla burada kullanmayız. soyut olanları yani başında I olanları burada kullanırız.
        //yani burada newlemek yok!.
        //refactoring yapıyoruz

        //HttpGet demek. ben buraya bir internet bağlantısı ile request göndereceğim sende bana o bağlantı ile response ver.
        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            //burda dependecy chain vardır. yani IProductService productmanagere bağlı o da productdal a bağlı.
            //IProductService productService = new ProductManager(new EfProductDal());
            var result = _productService.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
            //eğer OK verirsek yani 200 datamızı göster ama eğer 400 erroru verirsek hata mesajımızı yolla.
            //burada sunucu tarafondan bir request geldiğinde ki burada GetAll() bütün dataları getir demektir
            //bize o sonucu döndürür. fakat biz burayı refactor edeceğiz. öncelikle bu şekilde görelim.
        }

        [HttpGet("getbyid")]
        public IActionResult GetById(int id)
        {
            var result = _productService.GeyById(id);
            if(result.Success)
            {
                return Ok(result);
            }else
            {
                return BadRequest(result);
            }
        }

        //önceki get idi yani veriyi görmek içindir
        //şimdi ise post yani postlamak, eklemek için olan kod bölümümüz.
        [HttpPost("add")]
        public IActionResult Add(Product product)
        {
            var result = _productService.Add(product);
            if (result.Success)
            {
                return Ok(result);
            }else
            {
                return BadRequest(result);
            }
        }

    }
}
