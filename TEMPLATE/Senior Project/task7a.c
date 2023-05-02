#include <stdio.h>

int lcm(int n , int m , int z , int multiple){
    if( (n % multiple == 0) && (m % multiple == 0) && (z % multiple == 0)){
        return multiple;
    }
    return lcm(n , m , z , multiple + 1);
}


int main(){

    int x , y , z;
    printf("Enter the number n :");
    scanf("%d" , &x);

    printf("Enter the number m :");
    scanf("%d" , &y);

    printf("Enter the number z :");
    scanf("%d" , &z);

    printf("LCM = %d" , lcm(x , y , z , 2));

    return 0;
}