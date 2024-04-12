﻿using SonicSpectrum.Application.DTOs;
using SonicSpectrum.Application.Models;
using SonicSpectrum.Domain.Entities;

namespace SonicSpectrum.Application.Repository.Abstract
{
    public  interface IMusicSettingService
    {

        #region Add

        Task<OperationResult> AddAlbumAsync(AlbumDto albumDto);
        Task<OperationResult> AddArtistAsync(ArtistDTO artistDto);
        Task<OperationResult> AddGenreAsync(GenreDTO genreDto);
        Task<OperationResult> AddTrackAsync(TrackDTO trackDto);

        #endregion

        #region edit

        Task<OperationResult> EditAlbumAsync(string albumId, AlbumDto albumDto);
        Task<OperationResult> EditArtistAsync(string artistId, ArtistDTO artistDto);
        Task<OperationResult> EditGenreAsync(string genreId, GenreDTO genreDto);
        Task<OperationResult> EditTrackAsync(string trackId, TrackDTO trackDto);

        #endregion

        #region Delete

        Task<OperationResult> DeleteArtistAsync(string artistId);
        Task<OperationResult> DeleteAlbumAsync(string albumId);
        Task<OperationResult> DeleteGenreAsync(string genreId);
        Task<OperationResult> DeleteTrackAsync(string trackId);

        #endregion

        #region get

        Task<IEnumerable<object>> GetAllTracksAsync(int pageNumber, int pageSize);

        #endregion

    }
}
