using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria.GameContent;

namespace BetterNightSky
{
	public enum CelestialObject
	{
		Mars = 8,
		Saturn,
		Jupiter,
		Mercury,
		CrabNebula,
		Andromeda,
		CatsEyeNebula,
		CarinaNebula,
		Triangulum,
		LargeMagellanicCloud,
		SmallMagellanicCloud,
		HelixNebula,
		Uranus
	}

	class BetterNightSky : Mod
	{
		public static Dictionary<CelestialObject, int> CelestialIndex = new();
		public static int NewArraySize = 21;
		public static int NewStarCount = 1200;

		//we use a new random object because Main.rand isn't initialized yet in Load()
		static Random temp_Rand = new();

		public BetterNightSky()
		{
		}

		public override void Load()
		{
			if (Main.dedServ)
				return;
			
			Array.Resize(ref TextureAssets.Star, NewArraySize);

			SpawnNewStars();

			for (int i = 0; i < TextureAssets.Star.Length; i++)
			{
				TextureAssets.Star[i] = Assets.Request<Texture2D>("Textures/Star_" + i);
			}

			for (int i = 0; i < TextureAssets.Moon.Length; i++)
			{
				TextureAssets.Moon[i] = Assets.Request<Texture2D>("Textures/Moon1");
			}

			for (int i = 0; i < Main.star.Length; i++)
			{
				if (temp_Rand.Next(17) == 0)
				{
					Main.star[i].type = 5;
				}

				if (temp_Rand.Next(13) == 0)
				{
					Main.star[i].type = 7;
				}
			}

			for (int i = 8; i < NewArraySize; i++)
			{
				Main.star[450 - (i - 7)].type = i;
				CelestialIndex.Add((CelestialObject) i, 450 - (i - 7));
			}
		}

		public override void Unload()
		{
			CelestialIndex.Clear();
			Main.numStars = 0;

			if (!Main.dedServ)
			{
				// 0, 1, 2, 3. There is a sprite for the 5th star type, but Terraria's code only counts until the 4th
				const int originalStarTypes = 4;
				
				Array.Resize(ref TextureAssets.Star, originalStarTypes);
				Array.Resize(ref Main.star, 400);

				Main.numStars = 300;
				Main.rand = new Terraria.Utilities.UnifiedRandom();
				Star.SpawnStars();

				for (int i = 0; i < Main.star.Length; i++)
				{
					if (Main.star[i].type > (originalStarTypes - 1))
					{
						Main.star[i].type = Main.rand.Next(originalStarTypes);
					}
				}

				for (int i = 0; i < originalStarTypes; i++)
				{
					TextureAssets.Star[i] = Main.Assets.Request<Texture2D>("Images/Star_" + i);
				}

				for (int i = 0; i < TextureAssets.Moon.Length; i++)
				{
					TextureAssets.Moon[i] = Main.Assets.Request<Texture2D>("Images/Moon_" + i);
				}
			}
			Main.numStars = 130;
		}

		//Extracted from normal vanilla code because Main.numStars was hardcoded to 130 in there
		//so I just decided to be lazy and take this method out and set Main.numStars to whatever i want
		public static void SpawnNewStars()
		{
			Array.Resize<Star>(ref Main.star, NewStarCount);
			Main.numStars = NewStarCount;
			for (int i = 0; i < Main.numStars; i++)
			{
				Main.star[i] = new Star();
				Main.star[i].position.X = (float) temp_Rand.Next(-12, Main.screenWidth + 1);
				Main.star[i].position.Y = (float) temp_Rand.Next(-12, Main.screenHeight);
				Main.star[i].rotation = (float) temp_Rand.Next(628) * 0.01f;
				Main.star[i].scale = (float) temp_Rand.Next(50, 120) * 0.01f;
				Main.star[i].type = temp_Rand.Next(0, 5);
				Main.star[i].twinkle = (float) temp_Rand.Next(101) * 0.01f;
				Main.star[i].twinkleSpeed = (float) temp_Rand.Next(40, 100) * 0.0001f;
				if (temp_Rand.Next(2) == 0)
				{
					Main.star[i].twinkleSpeed *= -1f;
				}
				Main.star[i].rotationSpeed = (float) temp_Rand.Next(10, 40) * 0.0001f;
				if (temp_Rand.Next(2) == 0)
				{
					Main.star[i].rotationSpeed *= -1f;
				}
			}
		}
	}
}
