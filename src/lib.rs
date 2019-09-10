mod vec3;
mod spline;

use vec3::Vec3;
use spline::CatMullRommSpline;

#[no_mangle]
pub extern fn catmull_rom_spline_new(closed: bool) -> *const CatMullRommSpline {
    let spline = CatMullRommSpline::new(closed);
    &spline as *const CatMullRommSpline
}

#[no_mangle]
pub extern fn catmull_romm_spline_add_control_point(spline: *const CatMullRommSpline, p: Vec3) {
    let spline = unsafe { &*spline };
    spline.add_control_point(p);
}

#[no_mangle]
pub extern fn catmull_romm_spline_set_control_point(spline: *const CatMullRommSpline, i: usize, p: Vec3) {
    let spline = unsafe { &*spline };
    spline.set_control_points(i, p);
}

#[no_mangle]
pub extern fn catmull_romm_spline_calculate_point(spline: *const CatMullRommSpline, t: f32) -> Vec3 {
    let spline = unsafe { &*spline };
    spline.calculate_point(t)
}
