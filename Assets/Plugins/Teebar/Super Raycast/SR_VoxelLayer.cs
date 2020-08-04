using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SR_VoxelLayer : MonoBehaviour
{
	public struct Voxel
	{
		public int x;
		public int y;
		public int z;
	}

	[Range(1, 1000)] public int voxelSize = 10;

	public Voxel GetVoxel(Vector3 point)
	{
		var v = new Voxel();
		v.x = Mathf.RoundToInt(point.x / voxelSize);
		v.y = Mathf.RoundToInt(point.y / voxelSize);
		v.z = Mathf.RoundToInt(point.z / voxelSize);
		return v;
	}

	/*public List<Voxel> GetVoxelsOnLine(Voxel v0, Voxel v1)
	{
		var gx0idx = Math.floor(gx0);
		var gy0idx = Math.floor(gy0);
		var gz0idx = Math.floor(gz0);

		var gx1idx = Math.floor(gx1);
		var gy1idx = Math.floor(gy1);
		var gz1idx = Math.floor(gz1);

		var sx = gx1idx > gx0idx ? 1 : gx1idx < gx0idx ? -1 : 0;
		var sy = gy1idx > gy0idx ? 1 : gy1idx < gy0idx ? -1 : 0;
		var sz = gz1idx > gz0idx ? 1 : gz1idx < gz0idx ? -1 : 0;

		var gx = gx0idx;
		var gy = gy0idx;
		var gz = gz0idx;

		//Planes for each axis that we will next cross
		var gxp = gx0idx + (gx1idx > gx0idx ? 1 : 0);
		var gyp = gy0idx + (gy1idx > gy0idx ? 1 : 0);
		var gzp = gz0idx + (gz1idx > gz0idx ? 1 : 0);

		//Only used for multiplying up the error margins
		var vx = gx1 === gx0 ? 1 : gx1 - gx0;
		var vy = gy1 === gy0 ? 1 : gy1 - gy0;
		var vz = gz1 === gz0 ? 1 : gz1 - gz0;

		//Error is normalized to vx * vy * vz so we only have to multiply up
		var vxvy = vx * vy;
		var vxvz = vx * vz;
		var vyvz = vy * vz;

		//Error from the next plane accumulators, scaled up by vx*vy*vz
		// gx0 + vx * rx === gxp
		// vx * rx === gxp - gx0
		// rx === (gxp - gx0) / vx
		var errx = (gxp - gx0) * vyvz;
		var erry = (gyp - gy0) * vxvz;
		var errz = (gzp - gz0) * vxvy;

		var derrx = sx * vyvz;
		var derry = sy * vxvz;
		var derrz = sz * vxvy;

		do {
			visitor(gx, gy, gz);

			if (gx === gx1idx && gy === gy1idx && gz === gz1idx) break;

			//Which plane do we cross first?
			var xr = Math.abs(errx);
			var yr = Math.abs(erry);
			var zr = Math.abs(errz);

			if (sx !== 0 && (sy === 0 || xr < yr) && (sz === 0 || xr < zr)) {
				gx += sx;
				errx += derrx;
			}
			else if (sy !== 0 && (sz === 0 || yr < zr)) {
				gy += sy;
				erry += derry;
			}
			else if (sz !== 0) {
				gz += sz;
				errz += derrz;
			}

		} while (true);
	}*/

	#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		var camPos = UnityEditor.SceneView.lastActiveSceneView.camera.transform.position;

	}
	#endif
}
