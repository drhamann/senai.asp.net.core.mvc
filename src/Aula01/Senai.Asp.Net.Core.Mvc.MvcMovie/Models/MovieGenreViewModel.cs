using Microsoft.AspNetCore.Mvc.Rendering;

namespace Senai.Asp.Net.Core.Mvc.MvcMovie.Models;
public class MovieGenreViewModel
{
    public List<Movie>? Movies { get; set; }
    public SelectList? Genres { get; set; }
    public string? MovieGenre { get; set; }
    public string? SearchString { get; set; }
}
