using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Figgle;

using Domain.Services;
using Domain.Entities;
using Domain.Repositories;
using Domain.Errors;

namespace Application.ConverterService
{
    public class ConverterService : IConverterService
    {
        private IRepositoryFactory _repositoryFactory;

        string[] _font_names = { "1row", "alphabet", "graffiti", "isometric1", "threepoint", "weird", "rectangles" };

        public ConverterService(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory ?? throw new ArgumentNullException("repository factory");
        }

        public async Task<string> ConvertFont(int fontId, string inputText)
        {
            await SetupFonts();
            if (inputText == null || inputText.Length == 0) return "";

            var fontRepo = _repositoryFactory.CreateFontRepository();
            var domain_font = await fontRepo.GetFontById(fontId);
            var fontName = domain_font.Name;

            FiggleFont font = FiggleFonts.TryGetByName(fontName);
            if (font == null)
            {
                throw new ClientErrorException("No such font");
            }
            return font.Render(inputText);
        }

        public async Task<IEnumerable<string>> GetFontNames()
        {
            await SetupFonts();
            var fontRepo = _repositoryFactory.CreateFontRepository();
            var fonts = await fontRepo.GetFonts();
            var fontNames = new List<string>();
            foreach (var item in fonts)
                fontNames.Add(item.Name);
            return fontNames;
        }

        public async Task<IEnumerable<Convertion>> GetConvertions()
        {
            var cvtRepo = _repositoryFactory.CreateConvertionRepository();
            return await cvtRepo.GetConvertions();
        }

        public async Task<Convertion> GetConvertionById(int id)
        {
            var cvtRepo = _repositoryFactory.CreateConvertionRepository();
            return await cvtRepo.GetConvertionById(id);
        }

        public async Task<int> AddConvertion(Convertion cvt)
        {
            var cvtRepo = _repositoryFactory.CreateConvertionRepository();
            return await cvtRepo.Create(cvt);
        }

        public async Task<IEnumerable<Domain.Entities.Font>> GetFonts()
        {
            await SetupFonts();
            var fontRepo = _repositoryFactory.CreateFontRepository();
            return await fontRepo.GetFonts();
        }

        public async Task<Domain.Entities.Font> GetFontById(int id)
        {
            await SetupFonts();
            var fontRepo = _repositoryFactory.CreateFontRepository();
            return await fontRepo.GetFontById(id);
        }

        public async Task UpdateFont(Domain.Entities.Font font)
        {
            await SetupFonts();
            var fontRepo = _repositoryFactory.CreateFontRepository();
            var tmp = await fontRepo.GetFontById(font.Id);
            if (tmp == null)
                throw new NotFoundException("No font with such id");

            fontRepo = _repositoryFactory.CreateFontRepository();
            await fontRepo.Update(font);
        }

        public async Task DeleteFont(int id)
        {
            var fontRepo = _repositoryFactory.CreateFontRepository();
            var font = await fontRepo.GetFontById(id);
            if (font == null)
                throw new NotFoundException("No font with such id");

            fontRepo = _repositoryFactory.CreateFontRepository();
            await fontRepo.Delete(id);
        }

        private async Task SetupFonts()
        {
            var fontRepo = _repositoryFactory.CreateFontRepository();
            var fonts = new List<Font>(await fontRepo.GetFonts());
            if (fonts.Count == 0)
            {
                foreach (var name in _font_names)
                {
                    fontRepo = _repositoryFactory.CreateFontRepository();
                    await fontRepo.Create(new Font
                    {
                        Name = name,
                    });
                }
            }
        }
    }
}
