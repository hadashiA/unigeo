use std::ops::{Add, Sub, Mul, Div, AddAssign, Index, IndexMut, Neg};

#[derive(Debug, PartialEq, Clone, Copy)]
pub struct Vec3 {
    e: [f32; 3]
}

impl Index<usize> for Vec3 {
    type Output = f32;

    fn index(&self, index: usize) -> &Self::Output {
        &self.e[index]
    }
}

impl IndexMut<usize> for Vec3 {
    fn index_mut(&mut self, index: usize) -> &mut Self::Output {
        &mut self.e[index]
    }
}

impl Add for Vec3 {
    type Output = Vec3;

    fn add(self, rhs: Self) -> Self::Output {
        Vec3::new(
            self[0] + rhs[0],
            self[1] + rhs[1],
            self[2] + rhs[2])
    }
}

impl Sub for Vec3 {
    type Output = Vec3;

    fn sub(self, rhs: Self) -> Self::Output {
        Vec3::new(
            self[0] - rhs[0],
            self[1] - rhs[1],
            self[2] - rhs[2])
    }
}

impl Mul for Vec3 {
    type Output = Vec3;

    fn mul(self, rhs: Self) -> Self::Output {
        Vec3::new(
            self[0] * rhs[0],
            self[1] * rhs[1],
            self[2] * rhs[2])
    }
}

impl Mul<f32> for Vec3 {
    type Output = Vec3;

    fn mul(self, rhs: f32) -> Self::Output {
        Vec3::new(
            self[0] * rhs,
            self[1] * rhs,
            self[2] * rhs)
    }
}

impl Mul<Vec3> for f32 {
    type Output = Vec3;

    fn mul(self, rhs: Vec3) -> Self::Output {
        Vec3::new(
            self * rhs[0],
            self * rhs[1],
            self * rhs[2])
    }
}

impl Div for Vec3 {
    type Output = Vec3;

    fn div(self, rhs: Self) -> Self::Output {
        Vec3::new(
            self[0] / rhs[0],
            self[1] / rhs[1],
            self[2] / rhs[2])
    }
}

impl Div<f32> for Vec3 {
    type Output = Vec3;

    fn div(self, rhs: f32) -> Self::Output {
        Vec3::new(
            self[0] / rhs,
            self[1] / rhs,
            self[2] / rhs)
    }
}

impl Neg for Vec3 {
    type Output = Vec3;

    fn neg(self) -> Self::Output {
        Vec3::new(-self[0], -self[1], -self[2])
    }
}

impl AddAssign for Vec3 {
    fn add_assign(&mut self, rhs: Self) {
        self[0] += rhs[0];
        self[1] += rhs[1];
        self[2] += rhs[2];
    }
}

impl Vec3 {
    pub const ZERO: Vec3 = Vec3 { e: [0.0, 0.0, 0.0] };
    pub const ONE: Vec3 = Vec3 { e: [1.0, 1.0, 1.0] };

    pub fn new(e0: f32, e1: f32, e2: f32) -> Vec3 {
        Vec3 { e: [e0, e1, e2] }
    }

    pub fn dot(a: &Vec3, b: &Vec3) -> f32 {
        a[0] * b[0] + a[1] * b[1] + a[2] * b[2]
    }

    pub fn cross(a: &Vec3, b: &Vec3) -> Vec3 {
        Vec3::new(
            a[1] * b[2] - a[2] * b[1],
            -(a[0] * b[2] - a[2] * b[0]),
            a[0] * b[1] - a[1] * b[0])
    }

    pub fn lerp(a: Vec3, b: Vec3, t: f32) -> Vec3 {
        (1.0 - t) * a + t * b
    }

    pub fn x(&self) -> f32 {
        self[0]
    }

    pub fn y(&self) -> f32 {
        self[1]
    }

    pub fn z(&self) -> f32 {
        self[2]
    }

    pub fn r(&self) -> f32 {
        self[0]
    }

    pub fn g(&self) -> f32 {
        self[1]
    }

    pub fn b(&self) -> f32 {
        self[2]
    }

    pub fn length_squared(&self) -> f32 {
        self[0] * self[0] + self[1] * self[1] + self[2] * self[2]
    }

    pub fn length(&self) -> f32 {
        self.length_squared().sqrt()
    }

    pub fn normalized(&self) -> Vec3 {
        *self / self.length()
    }
}

#[cfg(test)]
mod test {
    use super::*;

    #[test]
    fn test_add() {
        let a = Vec3::new(1.0, 2.0, 3.0);
        let b = Vec3::new(2.0, 3.0, 4.0);
        let result = a + b;
        assert_eq!(result, Vec3::new(3.0, 5.0, 7.0));

        let mut a = Vec3::new(1.0, 2.0, 3.0);
        a += Vec3::new(2.0, 2.0, 4.0);
        assert_eq!(a, Vec3::new(3.0, 4.0, 7.0));
    }

    #[test]
    fn test_length() {
        let v = Vec3::new(1.0, 1.0, 1.0);
        let l = 3.0f32;
        assert_eq!(v.length(), l.sqrt());
    }

    #[test]
    fn test_neg() {
        let v = Vec3::new(1.0, 2.0, 3.0);
        assert_eq!(-v, Vec3::new(-1.0, -2.0, -3.0));
    }
}
