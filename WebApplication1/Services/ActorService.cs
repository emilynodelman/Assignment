using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WebApplication1.DAL;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class ActorService
    {
        public ResponseModel<List<Actor>> GetActors(string? name, int? minRank, int? maxRank, int page, int pageSize)
        {
            ResponseModel<List<Actor>> response = new ResponseModel<List<Actor>>();
            try
            {
                var query = InMemoryDatabase.Actors.AsQueryable();

                if (!string.IsNullOrEmpty(name))
                    query = query.Where(a => a.Name.Contains(name));

                if (minRank.HasValue)
                    query = query.Where(a => a.Rank >= minRank);

                if (maxRank.HasValue)
                    query = query.Where(a => a.Rank <= maxRank);

                var actorsList = query
                    .Select(a => new Actor { Id = a.Id, Name = a.Name })
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                response.Content = actorsList;
                response.StatusCode = HttpStatusCode.OK;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.Content = new List<Actor>();
                response.StatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                ErrorModel error = new ErrorModel(HttpStatusCode.BadRequest.ToString(), "Error in ActorService - GetActors", ex.Message);
            }
        }

        public Actor GetActorById(string id)
        {
            try
            {
                if(InMemoryDatabase.Actors.FirstOrDefault(a => a.Id == id) != null)
                {
                    return InMemoryDatabase.Actors.FirstOrDefault(a => a.Id == id);
                }
                else throw new Exception("Actor dowsnt exists.");

            }
            catch (Exception ex)
            {
                throw new Exception($"Error in ActorService - GetActorById, Exception : {ex}");
            }
        }

        public void AddActor(Actor actor)
        {
            try
            {
                if (InMemoryDatabase.Actors.Any(a => a.Id == actor.Id))
                    throw new Exception("Actor with the same ID already exists.");

                InMemoryDatabase.Actors.Add(actor);
            }
            catch(Exception ex)
            {
                throw new Exception($"Error in ActorService - AddActor, Exception : {ex}");
            }
        }

        public void UpdateActor(Actor actor)
        {
            try
            {
                var existingActor = InMemoryDatabase.Actors.FirstOrDefault(a => a.Id == actor.Id);
                if (existingActor == null)
                    throw new Exception("Actor not found.");

                if (InMemoryDatabase.Actors.Any(a => a.Rank == actor.Rank && a.Id != actor.Id))
                    throw new Exception("Duplicate rank.");

                existingActor.Name = actor.Name;
                existingActor.Rank = actor.Rank;
                existingActor.Type = actor.Type;
                existingActor.Details = actor.Details;
                existingActor.Rank = actor.Rank;
                existingActor.Source = actor.Source;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in ActorService - UpdateActor, Exception : {ex}");
            }
        }

        public void DeleteActor(string id)
        {
            try
            {
                var actorToRemove = InMemoryDatabase.Actors.FirstOrDefault(a => a.Id == id);
                if (actorToRemove != null)
                    InMemoryDatabase.Actors.Remove(actorToRemove);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in ActorService - DeleteActor, Exception : {ex}");
            }
        }
    }
}
