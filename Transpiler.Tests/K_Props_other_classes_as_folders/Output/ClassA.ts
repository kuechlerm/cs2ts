import { ClassB } from './ClassB';
import { ClassC } from './Sub/ClassC';

export interface ClassA {
    B: ClassB;
    C: ClassC;
}