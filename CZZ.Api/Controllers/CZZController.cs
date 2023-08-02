namespace CZZ.Api.Controllers;

[ApiController]
[OpenApiTags("出租豬 - 物件紀錄")]
[Route("[controller]/[action]")]
public class CZZController : Controller
{
    private readonly ICZZServiceWrapper _czzServiceWrapper;

    public CZZController(ICZZServiceWrapper czzServiceWrapper)
    {
        _czzServiceWrapper = czzServiceWrapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetPath()
    {
        var result = await _czzServiceWrapper.HouseObjectService.GetAllFilesPath();
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetObject([FromQuery] string? Date)
    {
        var result = await _czzServiceWrapper.HouseObjectService.GetObjectByDate(Date);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetNASFilesPath(string? Folder)
    {
        var result = await _czzServiceWrapper.NASService.GetAllFilesPath(Folder);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetNASFolderPath(string? Folder)
    {
        var result = await _czzServiceWrapper.NASService.GetAllFolderPath(Folder);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetNASOpenFile(string? FilePath)
    {
        var result = await _czzServiceWrapper.NASService.GetAllFolderPath(FilePath);

        return Ok(result);
    }
}