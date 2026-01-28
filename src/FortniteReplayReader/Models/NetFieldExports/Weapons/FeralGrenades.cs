using Unreal.Core.Attributes;
using Unreal.Core.Models.Enums;

namespace FortniteReplayReader.Models.NetFieldExports.Weapons;

[NetFieldExportGroup("/FeralCorgiGameplay/Gameplay/Items/FlashBang/B_Prj_FeralCorgi_FlashBangGrenade.B_Prj_FeralCorgi_FlashBangGrenade_C", minimalParseMode: ParseMode.Normal)]
public class FeralFlashBang : BaseProjectile
{
    public override string Type { get; set; } = "FeralFlashBang";
}

[NetFieldExportGroup("/FeralCorgiGameplay/Gameplay/Items/FragGrenade/B_Prj_FeralCorgi_FragGrenade.B_Prj_FeralCorgi_FragGrenade_C", minimalParseMode: ParseMode.Normal)]
public class FeralFragGrenade : BaseProjectile
{
    public override string Type { get; set; } = "FeralFragGrenade";
}

[NetFieldExportGroup("/FeralCorgiGameplay/Gameplay/Items/SmokeGrenade/B_Prj_FeralCorgi_SmokeGrenade.B_Prj_FeralCorgi_SmokeGrenade_C", minimalParseMode: ParseMode.Normal)]
public class FeralSmokeGrenade : BaseProjectile
{
    public override string Type { get; set; } = "FeralSmokeGrenade";
}

[NetFieldExportGroup("/FeralCorgiGameplay/Gameplay/Items/ReconGrenade/Prj_BGASpawner_FeralCorgi_Recon.Prj_BGASpawner_FeralCorgi_Recon_C", minimalParseMode: ParseMode.Normal)]
public class FeralReconGrenade : BaseProjectile
{
    public override string Type { get; set; } = "FeralReconGrenade";
}

[NetFieldExportGroup("/FeralCorgiGameplay/Gameplay/Items/ImpulseGrenade/B_Prj_FeralCorgi_KnockGrenade.B_Prj_FeralCorgi_KnockGrenade_C", minimalParseMode: ParseMode.Normal)]
public class FeralKnockGrenade : BaseProjectile
{
    public override string Type { get; set; } = "FeralKnockGrenade";
}

[NetFieldExportGroup("/FeralCorgiGameplay/Gameplay/Items/FireGrenade/B_Prj_FeralCorgi_FireGrenade.B_Prj_FeralCorgi_FireGrenade_C", minimalParseMode: ParseMode.Normal)]
public class FeralFireGrenade : BaseProjectile
{
    public override string Type { get; set; } = "FeralFireGrenade";
}

[NetFieldExportGroup("/FeralCorgiGameplay/Gameplay/Items/BubbleShield/B_Prj_FeralCorgi_BubbleGrenade.B_Prj_FeralCorgi_BubbleGrenade_C", minimalParseMode: ParseMode.Normal)]
public class FeralBubbleGrenade : BaseProjectile
{
    public override string Type { get; set; } = "FeralBubbleGrenade";
}

[NetFieldExportGroup("/FeralCorgiGameplay/Gameplay/Items/TheWall/Prj_Feral_TheWall.Prj_Feral_TheWall_C", minimalParseMode: ParseMode.Normal)]
public class FeralWallGrenade : BaseProjectile
{
    public override string Type { get; set; } = "FeralWallGrenade";
}

[NetFieldExportGroup("/FeralCorgiGameplay/Gameplay/Items/ProxMine/B_Prj_FeralCorgi_ProxMine.B_Prj_FeralCorgi_ProxMine_C", minimalParseMode: ParseMode.Normal)]
public class FeralProxMine : BaseProjectile
{
    public override string Type { get; set; } = "FeralProxMine";
}

