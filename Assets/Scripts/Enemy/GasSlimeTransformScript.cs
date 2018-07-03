using UnityEngine;

public class GasSlimeTransformScript : MonoBehaviour
{

    public GameObject cloudEffect_prefab;
    public GameObject cloudEffect_min_prefab;

    //public GameObject moveEffect_prefab;
    //public GameObject eyeEffect_prefab;
    public float hp_percent = 100;
    private float last_hp_percent;

    private float min_emission;
    private float min_radius;
    private float min_shape_x;
    private float min_shape_y;
    private float min_shape_z;
    private float min_startsize_min;
    private float min_startsize_max;

    private float scale_emission;
    private float scale_radius;
    private float scale_shape_x;
    private float scale_shape_y;
    private float scale_shape_z;
    private float scale_startsize_min;
    private float scale_startsize_max;

    ParticleSystem cloudeffect_particle;

    // Use this for initialization
    void Start()
    {

        var cloudEffect = Instantiate(cloudEffect_prefab, transform.position, Quaternion.identity);
        cloudEffect.transform.Rotate(-90, 0, 0);
        cloudEffect.transform.SetParent(transform);
        cloudeffect_particle = cloudEffect.GetComponent<ParticleSystem>();


        setMinValues();
        setScalingValues();


    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setHpPercent(float hp_percent)
    {
        this.hp_percent = hp_percent;
        calculateSize();
    }

    private void calculateSize()
    {
        float scale = hp_percent / 100;

        ParticleSystem.EmissionModule emissionModule = cloudeffect_particle.emission;
        emissionModule.rateOverTime = min_emission + (scale_emission * scale);

        ParticleSystem.ShapeModule shapeModule = cloudeffect_particle.shape;

        shapeModule.radius = min_radius + (scale_radius * scale);


        float x = min_shape_x + (scale_shape_x * scale);
        float y = min_shape_y + (scale_shape_y * scale);
        float z = min_shape_z + (scale_shape_z * scale);

        shapeModule.scale = new Vector3(x, y, z);

        ParticleSystem.MainModule mainModule = cloudeffect_particle.main;

        ParticleSystem.MinMaxCurve startSizeCurve = mainModule.startSize;

        startSizeCurve.constantMin = min_startsize_min + (scale_startsize_min * scale);
        startSizeCurve.constantMax = min_startsize_max + (scale_startsize_max * scale);

        mainModule.startSize = startSizeCurve;



    }


    private void setScalingValues()
    {

        ParticleSystem ps = cloudEffect_min_prefab.GetComponent<ParticleSystem>();
        scale_emission = cloudeffect_particle.emission.rateOverTimeMultiplier - ps.emission.rateOverTimeMultiplier;
        scale_radius = cloudeffect_particle.shape.radius - ps.shape.radius;
        scale_shape_x = cloudeffect_particle.shape.scale.x - ps.shape.scale.x;
        scale_shape_y = cloudeffect_particle.shape.scale.y - ps.shape.scale.y;
        scale_shape_z = cloudeffect_particle.shape.scale.z - ps.shape.scale.z;
        scale_startsize_min = cloudeffect_particle.main.startSize.constantMin - ps.main.startSize.constantMin;
        scale_startsize_max = cloudeffect_particle.main.startSize.constantMax - ps.main.startSize.constantMax;
    }


    private void setMinValues()
    {

        ParticleSystem ps = cloudEffect_min_prefab.GetComponent<ParticleSystem>();
        min_emission = ps.emission.rateOverTimeMultiplier;
        min_radius = ps.shape.radius;
        min_shape_x = ps.shape.scale.x;
        min_shape_y = ps.shape.scale.y;
        min_shape_z = ps.shape.scale.z;
        min_startsize_min = ps.main.startSize.constantMin;
        min_startsize_max = ps.main.startSize.constantMax;
    }
}
