use crate::vec3::Vec3;
use std::cell::RefCell;

fn cubic_hermite(p0: Vec3, p1: Vec3, v0: Vec3, v1: Vec3, t: f32) -> Vec3 {
    let c0 = 2.0 * p0 + -2.0 * p1 + v0 + v1;
    let c1 = -3.0 * p0 + 3.0 * p1 + -2.0 * v0 - v1;
    let c2 = v0;
    let c3 = p0;

    let t2 = t * t;
    let t3 = t2 * t;
    c0 * t3 + c1 * t2 + c2 * t + c3
}

pub struct CatMullRommSpline {
    control_points: RefCell<Vec<Vec3>>,
    closed: bool,
}

impl CatMullRommSpline {
    pub fn new(closed: bool) -> Self {
        CatMullRommSpline {
            control_points: RefCell::new(Vec::new()),
            closed,
        }
    }

    pub fn add_control_point(&self, p: Vec3) {
        self.control_points.borrow_mut().push(p);
    }

    pub fn set_control_points(&self, i: usize, p: Vec3) {
        self.control_points.borrow_mut()[i] = p;
    }

    pub fn clear(&self) {
        self.control_points.borrow_mut().clear()
    }

    pub fn calculate_point(&self, t: f32) -> Vec3 {
        let control_points = self.control_points.borrow();
        let l = control_points.len();
        let progress = (l as f32 - (if self.closed { 0.0 } else { 1.0 })) * t;

        let mut i = progress.floor() as usize;
        let mut weight = progress - i as f32;

        if !self.closed && weight.abs() < 0.0001 && i >= l - 1
        {
            i = l - 2;
            weight = 1.0;
        }

        let (p0, p1) = if self.closed && i >= l - 1 {
            // last to first point
            (control_points[l - 1], control_points[0])
        } else {
            (control_points[i], control_points[i + 1])
        };

        let v0 = if i > 0 {
            // prev to next point
            0.5 * (p1 - control_points[i - 1])

        } else if self.closed {
            // last to next point
            0.5 * (p1 - control_points[l - 1])
        }
        else {
            p1 - p0
        };

        let v1 = if i < l - 2 {
            0.5 * (control_points[i + 2] - p0)
        } else if self.closed {
            if i >= l - 1 {
                // last to second point
                0.5 * (control_points[1] - p0)
            } else {
                // second last to first point
                0.5 * (control_points[0] - p0)
            }
        } else {
            p1 - p0
        };

        cubic_hermite(p0, p1, v0, v1, t)
    }
}