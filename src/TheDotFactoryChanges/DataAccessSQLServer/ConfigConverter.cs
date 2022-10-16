using System;

namespace DataAccessSQLServer
{
    public static class ConfigConverter
    {
        public static DataAccessInterface.Configuration MapToBusinessEntity(UserConfig info)
        {
            return new DataAccessInterface.Configuration
            {
                bitLayout = Enum.Parse<DataAccessInterface.BitLayout>(info.BitLayout),
                bmpVisualizerChar = info.BmpVisualizerChar,
                byteFormat = Enum.Parse<DataAccessInterface.ByteFormat>(info.ByteFormat),
                byteLeadingString = info.ByteLeadingString,
                byteOrder = Enum.Parse<DataAccessInterface.ByteOrder>(info.ByteOrder),
                commentBlockEndString = info.CommentBlockEndString,
                commentBlockMiddleString = info.CommentBlockMiddleString,
                commentCharDescriptor = info.CommentCharDescriptor != 0,
                commentCharVisualizer = info.CommentCharVisualizer != 0,
                commentEndString = info.CommentEndString,
                commentStartString = info.CommentStartString,
                commentStyle = Enum.Parse<DataAccessInterface.CommentStyle>(info.CommentStyle),
                commentVariableName = info.CommentVariableName != 0,
                descCharHeight = Enum.Parse<DataAccessInterface.DescriptorFormat>(info.DescCharHeight),
                descCharWidth = Enum.Parse<DataAccessInterface.DescriptorFormat>(info.DescCharWidth),
                descFontHeight = Enum.Parse<DataAccessInterface.DescriptorFormat>(info.DescFontHeight),
                descImgHeight = Enum.Parse<DataAccessInterface.DescriptorFormat>(info.DescImgHeight),
                descImgWidth = Enum.Parse<DataAccessInterface.DescriptorFormat>(info.DescImgWidth),
                displayName = info.DisplayName,
                flipHorizontal = info.FlipHorizontal != 0,
                flipVertical = info.FlipVertical != 0,
                generateLookupArray = info.GenerateLookupArray != 0,
                generateLookupBlocks = info.GenerateLookupBlocks != 0,
                generateSpaceCharacterBitmap = info.GenerateSpaceCharacterBitmap != 0,
                Id = info.Id,
                lineWrap = Enum.Parse<DataAccessInterface.LineWrap>(info.LineWrap),
                lookupBlocksNewAfterCharCount = info.LookupBlocksNewAfterCharCount,
                minHeight = info.MinHeight,
                paddingRemovalHorizontal = Enum.Parse<DataAccessInterface.PaddingRemoval>(info.PaddingRemovalHorizontal),
                paddingRemovalVertical = Enum.Parse<DataAccessInterface.PaddingRemoval>(info.PaddingRemovalVertical),
                rotation = Enum.Parse<DataAccessInterface.Rotation>(info.Rotation),
                spaceGenerationPixels = info.SpaceGenerationPixels,
                varNfBitmaps = info.VarNfBitmaps,
                varNfCharInfo = info.VarNfCharInfo,
                varNfFontInfo = info.VarNfFontInfo,
                varNfHeight = info.VarNfHeight,
                varNfWidth = info.VarNfWidth,
            };
        }

        public static UserConfig MapFromBusinessEntity(DataAccessInterface.Configuration cfg)
        {
            return new UserConfig
            {
                BitLayout = cfg.bitLayout.ToString(),
                BmpVisualizerChar = cfg.bmpVisualizerChar,
                ByteFormat = cfg.byteFormat.ToString(),
                ByteLeadingString = cfg.byteLeadingString,
                ByteOrder = cfg.byteOrder.ToString(),
                CommentBlockEndString = cfg.commentBlockEndString,
                CommentBlockMiddleString = cfg.commentBlockMiddleString,
                CommentCharDescriptor = cfg.commentCharDescriptor ? 1 : 0,
                CommentCharVisualizer = cfg.commentCharVisualizer ? 1 : 0,
                CommentEndString = cfg.commentEndString,
                CommentStartString = cfg.commentStartString,
                CommentStyle = cfg.commentStyle.ToString(),
                CommentVariableName = cfg.commentVariableName ? 1 : 0,
                DescCharHeight = cfg.descCharHeight.ToString(),
                DescCharWidth = cfg.descCharWidth.ToString(),
                DescFontHeight = cfg.descFontHeight.ToString(),
                DescImgHeight = cfg.descImgHeight.ToString(),
                DescImgWidth = cfg.descImgWidth.ToString(),
                DisplayName = cfg.displayName,
                FlipHorizontal = cfg.flipHorizontal ? 1 : 0,
                FlipVertical = cfg.flipVertical ? 1 : 0,
                GenerateLookupArray = cfg.generateLookupArray ? 1 : 0,
                GenerateLookupBlocks = cfg.generateLookupBlocks ? 1 : 0,
                GenerateSpaceCharacterBitmap = cfg.generateSpaceCharacterBitmap ? 1 : 0,
                Id = cfg.Id,
                LineWrap = cfg.lineWrap.ToString(),
                LookupBlocksNewAfterCharCount = cfg.lookupBlocksNewAfterCharCount,
                MinHeight = cfg.minHeight,
                PaddingRemovalHorizontal = cfg.paddingRemovalHorizontal.ToString(),
                PaddingRemovalVertical = cfg.paddingRemovalVertical.ToString(),
                Rotation = cfg.rotation.ToString(),
                SpaceGenerationPixels = cfg.spaceGenerationPixels,
                Ui_id = cfg.UserId,
                VarNfBitmaps = cfg.varNfBitmaps,
                VarNfCharInfo = cfg.varNfCharInfo,
                VarNfFontInfo = cfg.varNfFontInfo,
                VarNfHeight = cfg.varNfHeight,
                VarNfWidth = cfg.varNfWidth
            };
        }
    }
}
