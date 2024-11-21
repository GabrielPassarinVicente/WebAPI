using SharpKml.Base;
using SharpKml.Dom;
using SharpKml.Engine;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class KmlService
{
    private readonly KmlFile _kmlFile;

    public KmlService(string filePath)
    {
        using (var stream = File.OpenRead(filePath))
        {
            _kmlFile = KmlFile.Load(stream);
        }
    }

    // Método para obter todos os Placemarks no KML
    public IEnumerable<Placemark> GetAllPlacemarks()
    {
        return _kmlFile.Root.Flatten().OfType<Placemark>();
    }

    // Método para obter valores únicos de um campo específico dentro de ExtendedData
    public IEnumerable<string> GetUniqueValues(string fieldName)
    {
        var placemarks = GetAllPlacemarks();
        return placemarks
               .Select(p => GetExtendedDataValue(p, fieldName))
               .Where(value => !string.IsNullOrEmpty(value))
               .Distinct();
    }

    // Método auxiliar para obter o valor de um campo ExtendedData de um Placemark
    private string GetExtendedDataValue(Placemark placemark, string key)
    {
        if (placemark.ExtendedData == null)
            return null;

        var data = placemark.ExtendedData.Data.FirstOrDefault(d => d.Name == key);
        return data?.Value;
    }

    // Método para obter Placemarks filtrados
    public IEnumerable<Placemark> GetFilteredPlacemarks(string cliente, string situacao, string bairro, string referencia, string ruaCruzamento)
    {
        var placemarks = GetAllPlacemarks();

        if (!string.IsNullOrEmpty(cliente))
        {
            placemarks = placemarks.Where(p => GetExtendedDataValue(p, "CLIENTE") == cliente);
        }

        if (!string.IsNullOrEmpty(situacao))
        {
            placemarks = placemarks.Where(p => GetExtendedDataValue(p, "SITUAÇÃO") == situacao);
        }

        if (!string.IsNullOrEmpty(bairro))
        {
            placemarks = placemarks.Where(p => GetExtendedDataValue(p, "BAIRRO") == bairro);
        }

        if (!string.IsNullOrEmpty(referencia) && referencia.Length >= 3)
        {
            placemarks = placemarks.Where(p => GetExtendedDataValue(p, "REFERENCIA")?.Contains(referencia) == true);
        }

        if (!string.IsNullOrEmpty(ruaCruzamento) && ruaCruzamento.Length >= 3)
        {
            placemarks = placemarks.Where(p => GetExtendedDataValue(p, "RUA/CRUZAMENTO")?.Contains(ruaCruzamento) == true);
        }

        return placemarks;
    }

    // Método para exportar Placemarks filtrados para um novo arquivo KML
    public string ExportFilteredPlacemarks(IEnumerable<Placemark> placemarks, string outputPath)
    {
        var document = new Document();
        foreach (var placemark in placemarks)
        {
            document.AddFeature(placemark);
        }

        var kml = new Kml { Feature = document };
        var kmlFile = KmlFile.Create(kml, false);

        using (var stream = File.Create(outputPath))
        {
            kmlFile.Save(stream);
        }

        return outputPath;
    }
}
