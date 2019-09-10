mod vec3;
mod spline;

use vec3::Vec3;
use spline::CatMullRommSpline;
use std::mem;

#[no_mangle]
pub extern fn catmull_romm_spline_new(closed: bool) -> *mut CatMullRommSpline {
    let spline = CatMullRommSpline::new(closed);
    let spline = Box::new(spline);

    Box::into_raw(spline)
}

#[no_mangle]
pub extern fn catmull_romm_spline_drop(spline: *mut CatMullRommSpline) {
    unsafe { Box::from_raw(spline) };
}

#[no_mangle]
pub extern fn catmull_romm_spline_closed(spline: *const CatMullRommSpline) -> bool {
    let spline = unsafe { &*spline };
    spline.closed()
}

#[no_mangle]
pub extern fn catmull_romm_spline_control_point_count(spline: *const CatMullRommSpline) -> usize {
    let spline = unsafe { &*spline };
    spline.control_point_count()
}

#[no_mangle]
pub extern fn catmull_romm_spline_get_control_point(spline: *const CatMullRommSpline, i: usize) -> Vec3 {
    let spline = unsafe { &*spline };
    spline.get_control_point(i)
}

#[no_mangle]
pub extern fn catmull_romm_spline_add_control_point(spline: *const CatMullRommSpline, p: Vec3) {
    let spline = unsafe { &*spline };
    spline.add_control_point(p);
}

#[no_mangle]
pub extern fn catmull_romm_spline_set_control_point(spline: *const CatMullRommSpline, i: usize, p: Vec3) {
    let spline = unsafe { &*spline };
    spline.set_control_point(i, p);
}

#[no_mangle]
pub extern fn catmull_romm_spline_calculate_point(spline: *const CatMullRommSpline, t: f32) -> Vec3 {
    let spline = unsafe { &*spline };
    spline.calculate_point(t)
}


#[cfg(test)]
mod tests {
    use crate::*;

    #[test]
    fn instantiate() {
        let ptr = catmull_romm_spline_new(false);
        catmull_romm_spline_drop(ptr);
    }
}