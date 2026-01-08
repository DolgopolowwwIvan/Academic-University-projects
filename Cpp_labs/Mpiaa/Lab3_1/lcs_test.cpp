#include "/home/billiejean/Documents/Proga/Cpp_labs/Mpiaa/catch.hpp"

#include "lcs.h"


TEST_CASE( "Both strings are empty" ) {
    CHECK( lcs_brute_force("", "") == "" );
    CHECK( lcs_dynamic("", "") == "" );
}

TEST_CASE( "One string is empty" ) {
    CHECK( lcs_brute_force("", "abcd") == "" );
    CHECK( lcs_brute_force("abcd", "") == "" );
    CHECK( lcs_dynamic("", "abcd") == "" );
    CHECK( lcs_dynamic("abcd", "") == "" );
}

TEST_CASE( "Equal strings" ) {
    CHECK( lcs_brute_force("abcd", "abcd") == "abcd" );
    CHECK( lcs_dynamic("abcd", "abcd") == "abcd" );
}

TEST_CASE( "Substring" ) {
    CHECK( lcs_brute_force("abab", "ab") == "ab" );
    CHECK( lcs_dynamic("abab", "ab") == "ab" );
}

TEST_CASE( "Substring in the middle" ) {
    CHECK( lcs_brute_force("xyaban", "abarab") == "aba" );
    CHECK( lcs_dynamic("xyaban", "abarab") == "aba" );
}


TEST_CASE( "Subsequences" ) {
    CHECK( lcs_brute_force("nahybser", "iunkayxbis") == "naybs" );
    CHECK( lcs_brute_force("z1artunx", "yz21rx") == "z1rx" );
    CHECK( lcs_brute_force("z1arxzyx1a", "yz21rx") == "z1rx" );
    CHECK( lcs_brute_force("yillnum", "numyjiljil") == "yill" );

    CHECK( lcs_dynamic("nahybser", "iunkayxbis") == "naybs" );
    CHECK( lcs_dynamic("z1artunx", "yz21rx") == "z1rx" );
    CHECK( lcs_dynamic("z1arxzyx1a", "yz21rx") == "z1rx" );
    CHECK( lcs_dynamic("yillnum", "numyjiljil") == "yill" );
}

TEST_CASE( "Reverse subsequence" ) {
    auto result1 = lcs_brute_force("xablar", "ralbax");
    CHECK( (result1 == "aba" || result1 == "ala") ); 

    auto result2 = lcs_dynamic("xablar", "ralbax");
    CHECK( (result2 == "aba" || result2 == "ala") ); 
}