﻿using ShareClassWebAPI.Entities;

namespace ShareClassWebAPI
{
    public static class DataContextSeed
    {
        public static async Task SeedAsync(DataContext dataContext)
        {
            try
            {
                dataContext.Database.EnsureCreated();

                if (dataContext.DBClassRoom.Any())
                {
                    return;
                }

                var classRooms = new ClassRoom[]
                {
                    new ClassRoom
                    {
                        Name = "Maths",
                        InvitationCode = Guid.NewGuid(),
                        Description = "Maths class",
                        Teacher_Name = "Ivanov",
                        Students_Count = 0,
                        Creation_Date = DateTime.Now
                    },
                    new ClassRoom
                    {
                        Name = "Russian",
                        InvitationCode = Guid.NewGuid(),
                        Description = "Russian class",
                        Teacher_Name = "Ivanov 2",
                        Students_Count = 0,
                        Creation_Date = DateTime.Now
                    }
                };

                foreach (var classRoom in classRooms)
                {
                    dataContext.DBClassRoom.Add(classRoom);
                }

                await dataContext.SaveChangesAsync();

                var homeTasks = new HomeTask[]
                {
                    new HomeTask
                    {
                        Name = "Maths hometask",
                        Description = "do this this and this",
                        Creation_Date = DateTime.Now,
                        Deadline_Date = DateTime.MaxValue,
                        ClassRoom = dataContext.DBClassRoom.First(classRoom => classRoom.Name == classRoom.Name)
                    }
                };

                foreach (var homeTask in homeTasks)
                {
                    dataContext.DBHomeTask.Add(homeTask);
                }

                await dataContext.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }
    }
}
