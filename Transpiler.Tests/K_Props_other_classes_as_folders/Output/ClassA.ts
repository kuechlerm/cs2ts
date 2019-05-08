import { ClassB } from './ClassB';
import { ClassC } from './Sub/ClassC';

export interface ClassA {
    b: ClassB;
    c: ClassC;
}