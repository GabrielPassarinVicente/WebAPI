using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[ApiController]
[Route("api/placemarks")]
public class PlacemarksController : ControllerBase
{
    private readonly KmlService _kmlService;

    public PlacemarksController()
    {
        // Substitua pelo caminho correto do seu arquivo KML
        _kmlService = new KmlService("DIRECIONADORES1.kml");
    }

    // Endpoint para exportar Placemarks filtrados para um novo arquivo KML
    [HttpPost("export")]
    public IActionResult ExportPlacemarks([FromBody] FilterRequest filters)
    {
        var placemarks = _kmlService.GetFilteredPlacemarks(
            filters.Cliente, filters.Situacao, filters.Bairro,
            filters.Referencia, filters.RuaCruzamento);

        // Caminho para salvar o arquivo exportado
        string outputPath = "filtered_output.kml";
        _kmlService.ExportFilteredPlacemarks(placemarks, outputPath);

        return Ok($"Arquivo KML exportado para: {outputPath}");
    }

    // Endpoint para listar Placemarks filtrados em formato JSON
    [HttpGet]
    public IActionResult ListPlacemarks([FromQuery] FilterRequest filters)
    {
        var placemarks = _kmlService.GetFilteredPlacemarks(
            filters.Cliente, filters.Situacao, filters.Bairro,
            filters.Referencia, filters.RuaCruzamento);

        return Ok(placemarks); // Converte os placemarks para JSON automaticamente
    }

    // Endpoint para obter valores únicos para os filtros
    [HttpGet("filters")]
    public IActionResult GetAvailableFilters()
    {
        var clientes = _kmlService.GetUniqueValues("CLIENTE");
        var situacoes = _kmlService.GetUniqueValues("SITUAÇÃO");
        var bairros = _kmlService.GetUniqueValues("BAIRRO");

        return Ok(new { Clientes = clientes, Situacoes = situacoes, Bairros = bairros });
    }
}

public class FilterRequest
{
    public string Cliente { get; set; }
    public string Situacao { get; set; }
    public string Bairro { get; set; }
    public string Referencia { get; set; }
    public string RuaCruzamento { get; set; }
}
