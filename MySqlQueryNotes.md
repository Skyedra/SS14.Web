As I haven't explored yet getting the admin interface up and running, here's my notes on a couple handy mysql queries:

# Adding a tracked IP to db:

```
INSERT INTO TrackedCommunityAddress SET TrackedCommunityId=1, StartAddressRange=INET6_ATON("1.2.3.4"), EndAddressRange=INET6_ATON("1.2.3.4"); 
```

# Listing tracked IPs in human readable format

```
SELECT Id, TrackedCommunityId, INET6_NTOA(`StartAddressRange`), INET6_NTOA(`EndAddressRange`) FROM `TrackedCommunityAddress` WHERE 1;
```

# Finding tracked community based on IP

```
SELECT * from TrackedCommunityAddress WHERE INET6_ATON("1.2.3.4") BETWEEN `StartAddressRange` and `EndAddressRange`
```